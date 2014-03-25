using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.XmlEvents;

namespace NXKit.XForms
{

    public class XFormsModule :
        Module
    {

        /// <summary>
        /// Map of <see cref="XName"/> to <see cref="NXNode"/> type.
        /// </summary>
        static readonly Dictionary<XName, Type> visualTypeMap = typeof(XFormsModule).Assembly.GetTypes()
               .Select(i => new { Type = i, Attribute = i.GetCustomAttribute<VisualAttribute>() })
               .Where(i => i.Attribute != null)
               .ToDictionary(i => Constants.XForms_1_0 + i.Attribute.Name, i => i.Type);

        /// <summary>
        /// Tracks whether the processor is currently executing an outermost action handler.
        /// </summary>
        internal bool executingOutermostActionHandler;

        public override Type[] DependsOn
        {
            get
            {
                return new[]
                {
                    typeof(EventsModule)
                };
            }
        }

        public override void Initialize(NXDocument engine)
        {
            base.Initialize(engine);

            Document.Changed += Document_Changed;
            Document.ProcessSubmit += Form_ProcessSubmit;
        }

        void Document_Changed(object sender, NXObjectChangeEventArgs args)
        {
            // objects added to document
            if (args.Change != NXObjectChange.Add)
                return;

            var element = args.Object as NXElement;
            if (element == null)
                return;

            // obtain all model visuals
            var models = element
                .Descendants(true)
                .OfType<XFormsModelVisual>()
                .ToList();

            foreach (var model in models)
            {
                // obtain instances
                var instances = model
                    .Descendants(false)
                    .OfType<XFormsInstanceVisual>()
                    .ToList();

                // initialize the instances
                foreach (var instance in instances)
                    instance.State.Initialize(model, instance);
            }

            // perform refresh of just loaded visuals
            if (models.All(i => i.State.Ready))
                foreach (var binding in Document.Root.Descendants(true).OfType<XFormsBindingVisual>())
                    binding.Refresh();
        }

        /// <summary>
        /// Creates the appropriate <see cref="NXNode"/> instance.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override NXNode CreateNode(XNode node)
        {
            var element = node as XElement;
            if (element == null)
                return null;

            if (element.Name.Namespace != Constants.XForms_1_0)
                return null;

            var type = visualTypeMap.GetOrDefault(element.Name);
            if (type == null)
                return null;

            return (XFormsVisual)Activator.CreateInstance(type, new object[] { node });
        }

        /// <summary>
        /// Resolves the XForms node for attribute <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal XAttribute ResolveAttribute(XElement element, string name)
        {
            if (element.Name.Namespace == Constants.XForms_1_0)
                // only xforms native elements support default-ns attributes
                return element.Attribute(Constants.XForms_1_0 + name) ?? element.Attribute(name);
            else
                // non-xforms native elements must be prefixed
                return element.Attribute(Constants.XForms_1_0 + name);
        }

        /// <summary>
        /// Gets the XForms attribute value <paramref name="name"/> on <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string GetAttributeValue(XElement element, string name)
        {
            var attr = ResolveAttribute(element, name);
            return attr != null ? attr.Value : null;
        }

        /// <summary>
        /// Obtains the model item properties for <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal XFormsModelItemState GetModelItem(XObject obj)
        {
            var modelItem = obj.Annotation<XFormsModelItemState>();
            if (modelItem == null)
                obj.AddAnnotation(modelItem = new XFormsModelItemState());

            return modelItem;
        }

        void Form_ProcessSubmit(object sender, EventArgs e)
        {
            Submit();
        }

        void VersionExceptionEventDefaultAction(XFormsVersionExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsVersionExceptionEvent.Name);
            Document.Root.GetState<XFormsModuleState>().Failed = true;
        }

        void LinkExceptionEventDefaultAction(XFormsLinkExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsLinkExceptionEvent.Name);
            Document.Root.GetState<XFormsModuleState>().Failed = true;
        }

        void BindingExceptionEventDefaultAction(XFormsBindingExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsBindingExceptionEvent.Name);
            Document.Root.GetState<XFormsModuleState>().Failed = true;
        }

        public override bool Invoke()
        {
            if (Document.Root.GetState<XFormsModuleState>().Failed)
                return false;

            var work = false;

            // obtain all model visuals
            var models = Document.Root
                .Descendants(true)
                .OfType<XFormsModelVisual>()
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    if (!model.State.Construct)
                    {
                        model.Interface<IEventTarget>().DispatchEvent(new XFormsModelConstructEvent(model).Event);
                        work = true;
                    }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.ConstructDone)
                        {
                            model.Interface<IEventTarget>().DispatchEvent(new XFormsModelConstructDoneEvent(model).Event);
                            work = true;
                        }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.Ready)
                        {
                            model.Interface<IEventTarget>().DispatchEvent(new XFormsReadyEvent(model).Event);
                            work = true;
                        }

            if (Document.Root.GetState<XFormsModuleState>().Failed)
                return work;

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.RebuildFlag))
                    if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new XFormsRebuildEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RecalculateFlag))
                    if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new XFormsRecalculateEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RevalidateFlag))
                    if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new XFormsRevalidateEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RefreshFlag))
                    if (!Document.Root.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new XFormsRefreshEvent(model).Event);
                    }
            }

            return work;
        }

        /// <summary>
        /// Loads the instance data associated with the given model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal void ProcessModelInstance(XFormsModelVisual model)
        {
            var target = model.Interface<IEventTarget>();
            if (target == null)
                throw new NullReferenceException();

            foreach (var instance in model.Instances)
            {
                // generate required 'id' attribute
                Document.GetElementId(instance.Xml);

                // extract instance values from xml
                var instanceSrc = GetAttributeValue(instance.Xml, "src");
                var instanceChildElements = instance.Xml.Elements().ToArray();

                if (!string.IsNullOrWhiteSpace(instanceSrc))
                {
                    try
                    {
                        // normalize uri with base
                        var u = new Uri(instanceSrc, UriKind.RelativeOrAbsolute);
                        if (instance.Xml.BaseUri.TrimToNull() != null && !u.IsAbsoluteUri)
                            u = new Uri(new Uri(instance.Xml.BaseUri), u);

                        // return resource as a stream
                        var resource = Document.Resolver.Get(u);
                        if (resource == null)
                            throw new FileNotFoundException("Could not load resource", instanceSrc);

                        // parse resource into new DOM
                        var instanceDataDocument = XDocument.Load(resource);

                        // add to model
                        instance.State.Initialize(model, instance, instanceDataDocument);
                    }
                    catch (UriFormatException)
                    {
                        target.DispatchEvent(new XFormsLinkExceptionEvent(model).Event);
                    }
                }
                else if (instanceChildElements.Length >= 2)
                {
                    // invalid number of child elements
                    target.DispatchEvent(new XFormsLinkExceptionEvent(model).Event);
                }
                else if (instanceChildElements.Length == 1)
                {
                    instance.State.Initialize(model, instance, new XDocument(instanceChildElements[0]));
                }
            }
        }

        internal void ReadyDefaultAction(Event ev)
        {
            var model = (XFormsModelVisual)ev.Target.Node;
            model.State.Ready = true;
        }

        /// <summary>
        /// Invoked when an xforms-refresh event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RefreshDefaultAction(Event ev)
        {
            RefreshModel((XFormsModelVisual)ev.Target.Node);
        }

        /// <summary>
        /// Invoked when an xforms-revalidate event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RevalidateDefaultAction(Event ev)
        {
            RevalidateModel((XFormsModelVisual)ev.Target.Node);
        }

        /// <summary>
        /// Invoked when an xforms-recalculate event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RecalculateDefaultAction(Event ev)
        {
            RecalculateModel((XFormsModelVisual)ev.Target.Node);
        }

        /// <summary>
        /// Invoked when an xforms-rebuild event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RebuildDefaultAction(Event ev)
        {
            RebuildModel((XFormsModelVisual)ev.Target.Node);
        }

        /// <summary>
        /// Invoked when an xforms-reset event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void ResetDefaultAction(Event ev)
        {
            System.Console.WriteLine("xforms-reset");
        }

        /// <summary>
        /// Rebuilds the specified model.
        /// </summary>
        /// <param name="model"></param>
        internal void RebuildModel(XFormsModelVisual model)
        {
            do
            {
                model.State.RebuildFlag = false;
                model.State.RecalculateFlag = true;
            }
            while (model.State.RebuildFlag);
        }

        /// <summary>
        /// Recalculates the specified model.
        /// </summary>
        /// <param name="model"></param>
        internal void RecalculateModel(XFormsModelVisual model)
        {
            do
            {
                model.State.RecalculateFlag = false;
                model.State.RevalidateFlag = true;

                // for each each instance underneath the model
                foreach (var instance in model.Instances)
                {
                    var instanceModelItems = instance.State.Document.Root
                        .DescendantsAndSelf()
                        .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                        .Where(i => i is XElement || i is XAttribute)
                        .Select(i => new { Node = i, ModelItem = GetModelItem(i) });

                    // for each model item underneath the instance
                    foreach (var i in instanceModelItems)
                    {
                        var node = i.Node;
                        var modelItem = i.ModelItem;

                        if (modelItem.Remove)
                        {
                            if (node is XElement)
                                ((XElement)node).RemoveNodes();
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue("");
                            else
                                throw new Exception();

                            modelItem.Remove = false;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            model.State.RevalidateFlag = true;
                            model.State.RefreshFlag = true;
                        }

                        // model item contains a new value
                        if (modelItem.NewElement != null)
                        {
                            if (node is XElement)
                                ((XElement)node).ReplaceAll(modelItem.NewElement);
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue(modelItem.NewElement.Value);
                            else
                                throw new Exception();

                            modelItem.NewElement = null;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            model.State.RevalidateFlag = true;
                            model.State.RefreshFlag = true;
                        }

                        if (modelItem.NewValue != null)
                        {
                            if (node is XElement)
                                ((XElement)node).SetValue(modelItem.NewValue);
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue(modelItem.NewValue);
                            else
                                throw new Exception();

                            modelItem.NewValue = null;
                            modelItem.DispatchValueChanged = true;

                            // prompt model to act
                            model.State.RevalidateFlag = true;
                            model.State.RefreshFlag = true;
                        }
                    }
                }

                // apply binding expressions
                foreach (var bind in model.Descendants(false).OfType<XFormsBindVisual>())
                {
                    bind.Refresh();

                    if (bind.Binding == null ||
                        bind.Binding.Nodes == null ||
                        bind.Binding.Nodes.Length == 0)
                        continue;

                    var typeAttr = GetAttributeValue(bind.Xml, "type");
                    var readonlyAttr = GetAttributeValue(bind.Xml, "readonly");
                    var requiredAttr = GetAttributeValue(bind.Xml, "required");
                    var relevantAttr = GetAttributeValue(bind.Xml, "relevant");
                    var calculateAttr = GetAttributeValue(bind.Xml, "calculate");

                    for (int i = 0; i < bind.Binding.Nodes.Length; i++)
                    {
                        var node = bind.Binding.Nodes[i];
                        var modelItem = GetModelItem(node);

                        var ec = new XFormsEvaluationContext(bind.Binding.Context.Model, bind.Binding.Context.Instance, node, i + 1, bind.Binding.Nodes.Length);
                        var nc = new XFormsXsltContext(bind, ec);

                        // get existing values
                        var oldReadOnly = GetModelItemReadOnly(node);
                        var oldRequired = GetModelItemRequired(node);
                        var oldRelevant = GetModelItemRelevant(node);

                        if (typeAttr != null)
                        {
                            // lookup namespace of type specifier
                            var st = typeAttr.Split(':');
                            var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
                            var lp = st.Length == 2 ? st[1] : st[0];
                            modelItem.Type = XName.Get(lp, ns);
                        }

                        // calculate before setting read-only, so read-only can be overridden
                        if (calculateAttr != null)
                        {
                            // calculated nodes are readonly
                            modelItem.ReadOnly = true;

                            var calculateBinding = new XFormsBinding(bind, ec, calculateAttr);
                            if (calculateBinding.Value != null)
                            {
                                var oldValue = GetModelItemValue(ec, node);
                                if (oldValue != calculateBinding.Value)
                                {
                                    modelItem.NewValue = calculateBinding.Value;
                                    modelItem.DispatchValueChanged = true;
                                    model.State.RecalculateFlag = true;
                                }
                            }
                        }

                        // recalculate read-only value
                        if (readonlyAttr != null)
                        {
                            var obj = EvaluateXPath(bind, ec, readonlyAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.ReadOnly = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.ReadOnly = bool.Parse((string)obj);
                            else
                                throw new Exception();
                        }

                        // recalculate required value
                        if (requiredAttr != null)
                        {
                            var obj = EvaluateXPath(bind, ec, requiredAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Required = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Required = bool.Parse((string)obj);
                        }

                        // recalculate relevant value
                        if (relevantAttr != null)
                        {
                            var obj = EvaluateXPath(bind, ec, relevantAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Relevant = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Relevant = bool.Parse((string)obj);
                        }

                        // get new read-only value; raise event on change
                        var readOnly = GetModelItemReadOnly(node);
                        if (readOnly != oldReadOnly)
                            if (readOnly)
                                modelItem.DispatchReadOnly = true;
                            else
                                modelItem.DispatchReadWrite = true;

                        // get new required value; raise event on change
                        var required = GetModelItemRequired(node);
                        if (required != oldRequired)
                            if (required)
                                modelItem.DispatchRequired = true;
                            else
                                modelItem.DispatchOptional = true;

                        // get new relevant value; raise event on change
                        var relevant = GetModelItemRelevant(node);
                        if (relevant != oldRelevant)
                            if (relevant)
                                modelItem.DispatchEnabled = true;
                            else
                                modelItem.DispatchDisabled = true;
                    }
                }
            }
            while (model.State.RecalculateFlag);
        }

        /// <summary>
        /// Revalidates the specified model.
        /// </summary>
        /// <param name="model"></param>
        internal void RevalidateModel(XFormsModelVisual model)
        {
            do
            {
                model.State.RevalidateFlag = false;
                model.State.RefreshFlag = true;

                // apply binding expressions
                foreach (var bind in model.Descendants(false).OfType<XFormsBindVisual>())
                {
                    if (bind.Binding == null ||
                        bind.Binding.Nodes == null ||
                        bind.Binding.Nodes.Length == 0)
                        continue;

                    var constraintAttr = GetAttributeValue(bind.Xml, "constraint");
                    if (constraintAttr == null)
                        continue;

                    for (int i = 0; i < bind.Binding.Nodes.Length; i++)
                    {
                        var node = bind.Binding.Nodes[i];
                        var modelItem = GetModelItem(node);

                        var ec = new XFormsEvaluationContext(bind.Binding.Context.Model, bind.Binding.Context.Instance, node, i + 1, bind.Binding.Nodes.Length);

                        // get old valid value
                        var oldValid = GetModelItemValid(node);

                        // recalculate valid
                        var st = (string)EvaluateXPath(bind, ec, constraintAttr, XPathResultType.String);
                        if (!string.IsNullOrWhiteSpace(st))
                            modelItem.Valid = bool.Parse(st);

                        // get new valid value; raise event on change
                        var valid = GetModelItemValid(node);
                        if (valid != oldValid)
                            if (valid)
                                modelItem.DispatchValid = true;
                            else
                                modelItem.DispatchInvalid = true;
                    }
                }
            }
            while (model.State.RevalidateFlag);
        }

        /// <summary>
        /// Refreshes the specified model.
        /// </summary>
        /// <param name="model"></param>
        internal void RefreshModel(XFormsModelVisual model)
        {
            do
            {
                model.State.RefreshFlag = false;

                // resolve visuals whom are dependent on this model
                var visuals = Document.Root
                    .Descendants(true)
                    .OfType<XFormsBindingVisual>();

                // for each visual, dispatch required events
                foreach (var visual in visuals)
                {
                    var target = visual.Interface<IEventTarget>();
                    if (target == null)
                        throw new NullReferenceException();

                    // refresh underlying data
                    visual.Refresh();

                    var singleNodeBindingVisual = visual as XFormsSingleNodeBindingVisual;
                    if (singleNodeBindingVisual != null)
                    {
                        if (singleNodeBindingVisual.Binding == null)
                            continue;

                        var node = singleNodeBindingVisual.Binding.Node;
                        if (node == null)
                            continue;

                        var modelItem = GetModelItem(node);

                        // dispatch required events
                        if (modelItem.DispatchValueChanged)
                            target.DispatchEvent(new XFormsValueChangedEvent(visual).Event);
                        if (modelItem.DispatchValid)
                            target.DispatchEvent(new XFormsValidEvent(visual).Event);
                        if (modelItem.DispatchInvalid)
                            target.DispatchEvent(new XFormsInvalidEvent(visual).Event);
                        if (modelItem.DispatchEnabled)
                            target.DispatchEvent(new XFormsEnabledEvent(visual).Event);
                        if (modelItem.DispatchDisabled)
                            target.DispatchEvent(new XFormsDisabledEvent(visual).Event);
                        if (modelItem.DispatchOptional)
                            target.DispatchEvent(new XFormsOptionalEvent(visual).Event);
                        if (modelItem.DispatchRequired)
                            target.DispatchEvent(new XFormsRequiredEvent(visual).Event);
                        if (modelItem.DispatchReadOnly)
                            target.DispatchEvent(new XFormsReadOnlyEvent(visual).Event);
                        if (modelItem.DispatchReadWrite)
                            target.DispatchEvent(new XFormsReadWriteEvent(visual).Event);

                        // clear events
                        modelItem.DispatchValueChanged = false;
                        modelItem.DispatchValid = false;
                        modelItem.DispatchInvalid = false;
                        modelItem.DispatchEnabled = false;
                        modelItem.DispatchDisabled = false;
                        modelItem.DispatchOptional = false;
                        modelItem.DispatchRequired = false;
                        modelItem.DispatchReadOnly = false;
                        modelItem.DispatchReadWrite = false;

                        continue;
                    }
                }
            }
            while (model.State.RefreshFlag);
        }

        /// <summary>
        /// Resets the specified model.
        /// </summary>
        /// <param name="model"></param>
        internal void ResetModel(XFormsModelVisual model)
        {

        }

        /// <summary>
        /// Evaluates the given XPath expression.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="evaluationContext"></param>
        /// <param name="expression"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        internal object EvaluateXPath(NXNode visual, XFormsEvaluationContext evaluationContext, string expression, XPathResultType resultType)
        {
            Contract.Requires<ArgumentNullException>(visual != null);
            Contract.Requires<ArgumentNullException>(evaluationContext != null);
            Contract.Requires<ArgumentNullException>(expression != null);

            var nc = new XFormsXsltContext(visual, evaluationContext);
            var nv = ((XNode)evaluationContext.Node).CreateNavigator();
            var xp = XPathExpression.Compile(expression, nc);
            var nd = nv.Evaluate(xp);

            return ConvertXPath(nd, resultType);
        }

        /// <summary>
        /// Converts an XPath evaluation result into the specified type.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="resultType"></param>
        object ConvertXPath(object result, XPathResultType resultType)
        {
            if (result == null)
                return null;

            switch (resultType)
            {
                case XPathResultType.Number:
                    return Convert.ToDouble(result);
                case XPathResultType.Boolean:
                    return Convert.ToBoolean(result);
                case XPathResultType.String:
                    return Convert.ToString(result);
                default:
                    return result;
            }
        }

        /// <summary>
        /// Resolves the evaluation context inherited from parents of <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal XFormsEvaluationContext ResolveInScopeEvaluationContext(XFormsVisual visual)
        {
            XFormsEvaluationContext ec = null;

            // search up visual tree for initial context
            if (ec == null)
                ec = visual
                    .Ancestors()
                    .OfType<IEvaluationContextScope>()
                    .Select(i => i.Context)
                    .FirstOrDefault(i => i != null);

            // default to default model
            if (ec == null)
                ec = Document.Root
                    .Descendants(true)
                    .TakeWhile(i => !(i is XFormsGroupVisual))
                    .OfType<XFormsModelVisual>()
                    .Select(i => i.DefaultEvaluationContext)
                    .FirstOrDefault();

            return ec;
        }

        /// <summary>
        /// Resolves the <see cref="XFormsEvaluationContext"/> to be used by the given visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal XFormsEvaluationContext ResolveBindingEvaluationContext(XFormsVisual visual)
        {
            // attempt to retrieve model state given by 'model' attribute
            var modelAttr = GetAttributeValue(visual.Xml, "model");
            if (!string.IsNullOrWhiteSpace(modelAttr))
            {
                // find referenced model visual
                var model = Document.Root
                    .Descendants(true)
                    .TakeWhile(i => !(i is XFormsGroupVisual))
                    .OfType<XFormsModelVisual>()
                    .SingleOrDefault(i => Document.GetElementId(i.Xml) == modelAttr);

                if (model != null)
                    return model.Context;
                else
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new XFormsBindingExceptionEvent(visual).Event);
                    return null;
                }
            }

            return ResolveInScopeEvaluationContext(visual);
        }

        /// <summary>
        /// Resolves the single-node binding on <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal XFormsBinding ResolveSingleNodeBinding(XFormsBindingVisual visual)
        {
            var element = visual.Xml as XElement;
            if (element == null)
                return null;

            // attempt to resolve 'bind' attribute to bind element's context
            var bd = GetAttributeValue(visual.Xml, "bind");
            if (bd != null)
            {
                var bind = (XFormsBindVisual)visual.ResolveId(bd);

                // invalid bind element
                if (bind == null ||
                    bind.Context == null)
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new XFormsBindingExceptionEvent(visual).Event);
                    return null;
                }

                return bind.Binding;
            }

            // attempt to resolve 'ref' attribute
            var xp = GetAttributeValue(visual.Xml, "ref");
            if (xp != null)
            {
                var ec = ResolveBindingEvaluationContext(visual);
                if (ec == null)
                    return null;

                // otherwise continue by evaluating expression
                return new XFormsBinding(visual, ec, xp);
            }

            return null;
        }

        /// <summary>
        /// Resolves the node-set binding on <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal XFormsBinding ResolveNodeSetBinding(XFormsBindingVisual visual)
        {
            // attempt to resolve 'bind' attribute to bind element's context
            var bindAttr = GetAttributeValue(visual.Xml, "bind");
            if (bindAttr != null)
            {
                var bind = (XFormsBindVisual)visual.ResolveId(bindAttr);

                // invalid bind element
                if (bind == null ||
                    bind.Binding == null)
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new XFormsBindingExceptionEvent(visual).Event);
                    return null;
                }

                return bind.Binding;
            }

            var ec = ResolveBindingEvaluationContext(visual);
            if (ec != null)
            {
                var nodesetAttr = GetAttributeValue(visual.Xml, "nodeset");
                if (nodesetAttr != null)
                    return new XFormsBinding(visual, ec, nodesetAttr);
            }

            return null;
        }

        /// <summary>
        /// Gets the model visual of the specified <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        internal XFormsModelVisual GetModelItemModel(XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Document != null);
            Contract.Ensures(Contract.Result<XFormsModelVisual>() != null);

            return self.Document.Annotation<XFormsModelVisual>();
        }

        /// <summary>
        /// Gets the instance visual of the specified <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        internal XFormsInstanceVisual GetModelItemInstance(XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Document != null);
            Contract.Ensures(Contract.Result<XFormsInstanceVisual>() != null);

            return self.Document.Annotation<XFormsInstanceVisual>();
        }

        /// <summary>
        /// Clears the contents of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        internal void ClearModelItem(XFormsEvaluationContext ec, XObject item)
        {
            var mi = GetModelItem(item);
            mi.Remove = true;

            ec.Model.State.RecalculateFlag = true;
        }

        /// <summary>
        /// Sets the value of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        /// <param name="newElement"></param>
        internal void SetModelItemElement(XFormsEvaluationContext ec, XObject item, XElement newElement)
        {
            // register new value with model item
            GetModelItem(item).NewElement = newElement;

            // trigger recalculate event to collect new value
            ec.Model.State.RecalculateFlag = true;
            ec.Model.State.RevalidateFlag = true;
            ec.Model.State.RefreshFlag = true;
        }

        /// <summary>
        /// Sets the value of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        internal void SetModelItemValue(XFormsEvaluationContext ec, XObject item, string newValue)
        {
            var lastValue = GetModelItemValue(ec, item);
            if (lastValue == newValue)
                return;

            // register new value with model item
            var mi = GetModelItem(item);
            mi.NewValue = newValue;

            // trigger recalculate event to collect new value
            ec.Model.State.RecalculateFlag = true;
            ec.Model.State.RevalidateFlag = true;
            ec.Model.State.RefreshFlag = true;
        }

        /// <summary>
        /// Gets the value of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string GetModelItemValue(XFormsEvaluationContext ec, XObject item)
        {
            if (item is XElement)
                return ((XElement)item).Value;
            else if (item is XAttribute)
                return ((XAttribute)item).Value;
            else
                throw new Exception();
        }

        /// <summary>
        /// Returns the unique identifier of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal int GetModelItemId(XFormsEvaluationContext ec, XObject item)
        {
            var mi = GetModelItem(item);
            if (mi.Id == null)
                mi.Id = ec.Instance.GetState<XFormsInstanceVisualState>().AllocateItemId();

            return (int)mi.Id;
        }

        /// <summary>
        /// Returns the unique identifier for the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string GetModelItemUniqueId(XFormsEvaluationContext ec, XObject item)
        {
            Contract.Requires<ArgumentNullException>(ec != null);
            Contract.Requires<ArgumentNullException>(item != null);

            return GetAttributeValue(ec.Instance.Xml, "id") + "$" + GetModelItemId(ec, item);
        }

        /// <summary>
        /// Returns the type of the given instance data node.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal XName GetModelItemType(XObject item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            return GetModelItem(item).Type ?? NXKit.XmlSchemaConstants.XMLSchema + "string";
        }

        /// <summary>
        /// Returns whether or not the given instance data node is read-only.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemReadOnly(XObject item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            return item.AncestorsAndSelf().Any(i => GetModelItem(i).ReadOnly ?? false);
        }

        /// <summary>
        /// Returns whether or not the given instance data node is required.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemRequired(XObject item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            return GetModelItem(item).Required ?? false;
        }

        /// <summary>
        /// Returns whether or not the given model item is relevant.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemRelevant(XObject item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            return item.AncestorsAndSelf().All(i => GetModelItem(i).Relevant ?? true);
        }

        /// <summary>
        /// Returns whether or not the given instance data node is valid.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemValid(XObject item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            return GetModelItem(item).Valid ?? true;
        }

        /// <summary>
        /// Invokes the given action visual properly.
        /// </summary>
        /// <param name="visual"></param>
        internal void InvokeAction(IActionVisual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            var outermostAction = !executingOutermostActionHandler;
            if (outermostAction)
                executingOutermostActionHandler = true;

            visual.Invoke();

            if (outermostAction)
            {
                executingOutermostActionHandler = false;
                Invoke();
            }
        }

        internal void RaiseMessage(XFormsMessageVisual visual)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initiates a submission of all submission elements.
        /// </summary>
        public void Submit()
        {
            // ensure processor is up to date
            Document.Invoke();

            // all submission elements on the form
            var visuals = Document.Root
                .Descendants()
                .OfType<XFormsSubmissionVisual>();

            // raise a submit event for each submission
            foreach (var visual in visuals)
                visual.Interface<IEventTarget>().DispatchEvent(new XFormsSubmitEvent(visual).Event);
        }

    }

}
