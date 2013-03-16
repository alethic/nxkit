using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ISIS.Forms.XForms
{

    [Module]
    public class XFormsModule : Module
    {

        /// <summary>
        /// Tracks whether the processor is currently executing an outermost action handler.
        /// </summary>
        internal bool executingOutermostActionHandler;

        /// <summary>
        /// Initializes a new instance from the given state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="resolver"></param>
        [ImportingConstructor]
        public XFormsModule([Import(typeof(FormProcessor))] FormProcessor processor)
            : base(processor)
        {

        }

        public override void Initialize()
        {
            Form.ProcessSubmit += Form_ProcessSubmit;

            // obtain all model visuals
            var models = Form.RootVisual
                .Descendants(true)
                .OfType<XFormsModelVisual>()
                .ToList();

            // perform refresh of just loaded visuals
            if (models.All(i => i.State.Ready))
                foreach (var visual in Form.RootVisual.Descendants(true).OfType<XFormsBindingVisual>())
                    visual.Refresh();
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

        private void Form_ProcessSubmit(object sender, EventArgs e)
        {
            Submit();
        }

        private void VersionExceptionEventDefaultAction(XFormsVersionExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsVersionExceptionEvent.Name);
            Form.RootVisual.GetState<XFormsModuleState>().Failed = true;
        }

        private void LinkExceptionEventDefaultAction(XFormsLinkExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsLinkExceptionEvent.Name);
            Form.RootVisual.GetState<XFormsModuleState>().Failed = true;
        }

        private void BindingExceptionEventDefaultAction(XFormsBindingExceptionEvent ev)
        {
            System.Console.WriteLine(XFormsBindingExceptionEvent.Name);
            Form.RootVisual.GetState<XFormsModuleState>().Failed = true;
        }

        public override bool Run()
        {
            if (Form.RootVisual.GetState<XFormsModuleState>().Failed)
                return false;

            var work = false;

            // obtain all model visuals
            var models = Form.RootVisual
                .Descendants(true)
                .OfType<XFormsModelVisual>()
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    if (!model.State.Construct)
                    {
                        model.DispatchEvent<XFormsModelConstructEvent>();
                        work = true;
                    }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.ConstructDone)
                        {
                            model.DispatchEvent<XFormsModelConstructDoneEvent>();
                            work = true;
                        }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.Ready)
                        {
                            model.DispatchEvent<XFormsReadyEvent>();
                            work = true;
                        }

            if (Form.RootVisual.GetState<XFormsModuleState>().Failed)
                return work;

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.RebuildFlag))
                    if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.DispatchEvent<XFormsRebuildEvent>();
                    }

                foreach (var model in models.Where(i => i.State.RecalculateFlag))
                    if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.DispatchEvent<XFormsRecalculateEvent>();
                    }

                foreach (var model in models.Where(i => i.State.RevalidateFlag))
                    if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.DispatchEvent<XFormsRevalidateEvent>();
                    }

                foreach (var model in models.Where(i => i.State.RefreshFlag))
                    if (!Form.RootVisual.GetState<XFormsModuleState>().Failed)
                    {
                        work = true;
                        model.DispatchEvent<XFormsRefreshEvent>();
                    }
            }

            return work;
        }

        /// <summary>
        /// Loads the instance data associated with the given model.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal void ProcessModelInstance(XFormsModelVisual visual)
        {
            foreach (var instance in visual.Instances)
            {
                // generate required 'id' attribute
                Form.GetElementId(instance.Element);

                // extract instance values from xml
                var instanceSrc = GetAttributeValue(instance.Element, "src");
                var instanceChildElements = instance.Element.Elements().ToArray();

                if (!string.IsNullOrWhiteSpace(instanceSrc))
                {
                    try
                    {
                        // return resource as a stream
                        var resource = Resolver.Get(instanceSrc, instance.Element.BaseUri);

                        // parse resource into new DOM
                        var instanceDataDocument = FormProcessor.StringToXDocument(new StreamReader(resource).ReadToEnd(), null);

                        // add to model
                        instance.State.InstanceDocument = instanceDataDocument;
                        instance.State.InstanceElement = instanceDataDocument.Root;
                    }
                    catch (UriFormatException)
                    {
                        visual.DispatchEvent<XFormsLinkExceptionEvent>();
                        return;
                    }
                }
                else if (instanceChildElements.Length >= 2)
                {
                    // invalid number of child elements
                    visual.DispatchEvent<XFormsLinkExceptionEvent>();
                    return;
                }
                else if (instanceChildElements.Length == 1)
                {
                    var instanceDataNode = instanceChildElements.First();

                    // clone node into new document
                    var d = new XDocument(instanceDataNode);

                    // add to instance
                    instance.State.InstanceDocument = d;
                    instance.State.InstanceElement = d.Root;
                }
            }
        }

        internal void ReadyDefaultAction(XFormsReadyEvent ev)
        {
            var model = (XFormsModelVisual)ev.Target;
            model.State.Ready = true;
        }

        /// <summary>
        /// Invoked when an xforms-refresh event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RefreshDefaultAction(XFormsRefreshEvent ev)
        {
            RefreshModel((XFormsModelVisual)ev.Target);
        }

        /// <summary>
        /// Invoked when an xforms-revalidate event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RevalidateDefaultAction(XFormsRevalidateEvent ev)
        {
            RevalidateModel((XFormsModelVisual)ev.Target);
        }

        /// <summary>
        /// Invoked when an xforms-recalculate event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RecalculateDefaultAction(XFormsRecalculateEvent ev)
        {
            RecalculateModel((XFormsModelVisual)ev.Target);
        }

        /// <summary>
        /// Invoked when an xforms-rebuild event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void RebuildDefaultAction(XFormsRebuildEvent ev)
        {
            RebuildModel((XFormsModelVisual)ev.Target);
        }

        /// <summary>
        /// Invoked when an xforms-reset event is received by a model.
        /// </summary>
        /// <param name="ev"></param>
        internal void ResetDefaultAction(XFormsResetEvent ev)
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
                    var instanceModelItems = instance.State.InstanceElement
                        .DescendantsAndSelf()
                        .SelectMany(i => i.Attributes().Cast<XObject>().Prepend(i))
                        .Where(i => i is XElement || i is XAttribute)
                        .Select(i => new { Node = i, ModelItem = GetModelItem(i) });

                    // for each model item underneath the instance
                    foreach (var i in instanceModelItems)
                    {
                        var node = i.Node;
                        var modelItem = i.ModelItem;

                        if (modelItem.Clear)
                        {
                            if (node is XElement)
                                ((XElement)node).RemoveNodes();
                            else if (node is XAttribute)
                                ((XAttribute)node).SetValue("");
                            else
                                throw new Exception();

                            modelItem.Clear = false;
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

                    var typeAttr = GetAttributeValue(bind.Element, "type");
                    var readonlyAttr = GetAttributeValue(bind.Element, "readonly");
                    var requiredAttr = GetAttributeValue(bind.Element, "required");
                    var relevantAttr = GetAttributeValue(bind.Element, "relevant");
                    var calculateAttr = GetAttributeValue(bind.Element, "calculate");

                    for (int i = 0; i < bind.Binding.Nodes.Length; i++)
                    {
                        var node = bind.Binding.Nodes[i];
                        var modelItem = GetModelItem(node);

                        var ec = new XFormsEvaluationContext(bind.Binding.Context.Model, bind.Binding.Context.Instance, node, i + 1, bind.Binding.Nodes.Length);
                        var nc = new VisualXmlNamespaceContext(bind);

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

                            var calculateBinding = new XFormsBinding(Form, bind, ec, calculateAttr);
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
                            var obj = EvaluateXPath(ec, nc, bind, readonlyAttr, XPathResultType.Any);
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
                            var obj = EvaluateXPath(ec, nc, bind, requiredAttr, XPathResultType.Any);
                            if (obj is bool)
                                modelItem.Required = (bool)obj;
                            else if (obj is string && !string.IsNullOrWhiteSpace((string)obj))
                                modelItem.Required = bool.Parse((string)obj);
                        }

                        // recalculate relevant value
                        if (relevantAttr != null)
                        {
                            var obj = EvaluateXPath(ec, nc, bind, relevantAttr, XPathResultType.Any);
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

                    var constraintAttr = GetAttributeValue(bind.Element, "constraint");
                    if (constraintAttr == null)
                        continue;

                    for (int i = 0; i < bind.Binding.Nodes.Length; i++)
                    {
                        var node = bind.Binding.Nodes[i];
                        var modelItem = GetModelItem(node);

                        var ec = new XFormsEvaluationContext(bind.Binding.Context.Model, bind.Binding.Context.Instance, node, i + 1, bind.Binding.Nodes.Length);
                        var nc = new VisualXmlNamespaceContext(bind);

                        // get old valid value
                        var oldValid = GetModelItemValid(node);

                        // recalculate valid
                        var st = (string)EvaluateXPath(ec, nc, bind, constraintAttr, XPathResultType.String);
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
                var visuals = Form.RootVisual
                    .Descendants(true)
                    .OfType<XFormsBindingVisual>();

                // for each visual, dispatch required events
                foreach (var visual in visuals)
                {
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
                            visual.DispatchEvent<XFormsValueChangedEvent>();
                        if (modelItem.DispatchValid)
                            visual.DispatchEvent<XFormsValidEvent>();
                        if (modelItem.DispatchInvalid)
                            visual.DispatchEvent<XFormsInvalidEvent>();
                        if (modelItem.DispatchEnabled)
                            visual.DispatchEvent<XFormsEnabledEvent>();
                        if (modelItem.DispatchDisabled)
                            visual.DispatchEvent<XFormsDisabledEvent>();
                        if (modelItem.DispatchOptional)
                            visual.DispatchEvent<XFormsOptionalEvent>();
                        if (modelItem.DispatchRequired)
                            visual.DispatchEvent<XFormsRequiredEvent>();
                        if (modelItem.DispatchReadOnly)
                            visual.DispatchEvent<XFormsReadOnlyEvent>();
                        if (modelItem.DispatchReadWrite)
                            visual.DispatchEvent<XFormsReadWriteEvent>();

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
        /// <param name="node"></param>
        /// <param name="expression"></param>
        /// <param name="rn"></param>
        /// <returns></returns>
        internal object EvaluateXPath(XFormsEvaluationContext ec, VisualXmlNamespaceContext nc, XFormsVisual visual, string expression, XPathResultType resultType)
        {
            if (expression == null)
                return null;

            // put the evaluation context in thread-local scope for the duration of execution, so it's available to functions
            using (visual.Scope())
            using (ec.Scope())
            {
                var nv = ((XNode)ec.Node).CreateNavigator();
                var nd = ((XNode)ec.Node).XPathEvaluate(expression, nc);
                //var xp = XPathExpression.Compile(expression, nc);
                //var nd = nv.Evaluate(xp, nv.Select("."));

                return nd;
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
                ec = visual.Ascendants()
                    .OfType<IEvaluationContextScope>()
                    .Select(i => i.Context)
                    .FirstOrDefault(i => i != null);

            // default to default model
            if (ec == null)
                ec = Form.RootVisual
                    .Descendants(true)
                    .TakeWhile(i => !(i is XFormsGroupVisual))
                    .OfType<XFormsModelVisual>()
                    .First()
                    .DefaultEvaluationContext;

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
            var modelAttr = GetAttributeValue(visual.Element, "model");
            if (!string.IsNullOrWhiteSpace(modelAttr))
            {
                // find referenced model visual
                var model = Form.RootVisual
                    .Descendants(true)
                    .TakeWhile(i => !(i is XFormsGroupVisual))
                    .OfType<XFormsModelVisual>()
                    .SingleOrDefault(i => Form.GetElementId(i.Element) == modelAttr);

                if (model != null)
                    return model.Context;
                else
                {
                    visual.DispatchEvent<XFormsBindingExceptionEvent>();
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
            var element = visual.Node as XElement;
            if (element == null)
                return null;

            // attempt to resolve 'bind' attribute to bind element's context
            var bd = GetAttributeValue(visual.Element, "bind");
            if (bd != null)
            {
                var bind = (XFormsBindVisual)visual.ResolveId(bd);

                // invalid bind element
                if (bind == null ||
                    bind.Context == null)
                {
                    visual.DispatchEvent<XFormsBindingExceptionEvent>();
                    return null;
                }

                return bind.Binding;
            }

            // attempt to resolve 'ref' attribute
            var xp = GetAttributeValue(visual.Element, "ref");
            if (xp != null)
            {
                var ec = ResolveBindingEvaluationContext(visual);
                if (ec == null)
                    return null;

                // otherwise continue by evaluating expression
                return new XFormsBinding(Form, visual, ec, xp);
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
            var bindAttr = GetAttributeValue(visual.Element, "bind");
            if (bindAttr != null)
            {
                var bind = (XFormsBindVisual)visual.ResolveId(bindAttr);

                // invalid bind element
                if (bind == null ||
                    bind.Binding == null)
                {
                    visual.DispatchEvent<XFormsBindingExceptionEvent>();
                    return null;
                }

                return bind.Binding;
            }

            var ec = ResolveBindingEvaluationContext(visual);
            if (ec != null)
            {
                var nodesetAttr = GetAttributeValue(visual.Element, "nodeset");
                if (nodesetAttr != null)
                    return new XFormsBinding(Form, visual, ec, nodesetAttr);
            }

            return null;
        }

        /// <summary>
        /// Clears the contents of the given instance data node.
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="item"></param>
        internal void ClearModelItem(XFormsEvaluationContext ec, XObject item)
        {
            var mi = GetModelItem(item);
            mi.Clear = true;

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
                mi.Id = ++ec.Instance.GetState<XFormsInstanceVisualState>().NextItemId;

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
            return GetAttributeValue(ec.Instance.Element, "id") + "$" + GetModelItemId(ec, item);
        }

        /// <summary>
        /// Returns the type of the given instance data node.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal XName GetModelItemType(XObject item)
        {
            return GetModelItem(item).Type ?? FormConstants.XMLSchema + "string";
        }

        /// <summary>
        /// Returns whether or not the given instance data node is read-only.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemReadOnly(XObject item)
        {
            return item.AncestorsAndSelf().Any(i => GetModelItem(i).ReadOnly ?? false);
        }

        /// <summary>
        /// Returns whether or not the given instance data node is required.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemRequired(XObject item)
        {
            return GetModelItem(item).Required ?? false;
        }

        /// <summary>
        /// Returns whether or not the given model item is relevant.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemRelevant(XObject item)
        {
            return item.AncestorsAndSelf().All(i => GetModelItem(i).Relevant ?? true);
        }

        /// <summary>
        /// Returns whether or not the given instance data node is valid.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool GetModelItemValid(XObject item)
        {
            return GetModelItem(item).Valid ?? true;
        }

        /// <summary>
        /// Invokes the given action visual properly.
        /// </summary>
        /// <param name="visual"></param>
        internal void InvokeAction(IActionVisual visual)
        {
            var outermostAction = !executingOutermostActionHandler;
            if (outermostAction)
                executingOutermostActionHandler = true;

            visual.Invoke();

            if (outermostAction)
            {
                executingOutermostActionHandler = false;
                Run();
            }
        }

        internal void RaiseMessage(XFormsMessageVisual visual)
        {
            throw new NotImplementedException();

            //var ec = visual.BindingContext;
            //if (ec == null)
            //    return;

            //// add the visual to the set of messages to be raised
            //ec.Model.State.Messages.Add(visual);
        }

        /// <summary>
        /// Initiates a submission of all submission elements.
        /// </summary>
        public void Submit()
        {
            // ensure processor is up to date
            Form.Run();

            // all submission elements on the form
            var visuals = Form.RootVisual
                .Descendants()
                .OfType<XFormsSubmissionVisual>();

            // raise a submit event for each submission
            foreach (var visual in visuals)
                visual.DispatchEvent<XFormsSubmitEvent>();
        }

    }

}
