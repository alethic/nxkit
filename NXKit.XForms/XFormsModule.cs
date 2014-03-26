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

namespace NXKit.XForms
{

    public class XFormsModule :
        Module
    {

        /// <summary>
        /// Map of <see cref="XName"/> to <see cref="NXNode"/> type.
        /// </summary>
        static readonly Dictionary<XName, Type> visualTypeMap = typeof(XFormsModule).Assembly.GetTypes()
               .Select(i => new { Type = i, Attribute = i.GetCustomAttribute<ElementAttribute>() })
               .Where(i => i.Attribute != null)
               .ToDictionary(i => Constants.XForms_1_0 + i.Attribute.Name, i => i.Type);

        /// <summary>
        /// Tracks whether the processor is currently executing an outermost action handler.
        /// </summary>
        internal bool executingOutermostActionHandler;

        public override Type[] DependsOn
        {
            get { return new[] { typeof(DOMEventsModule) }; }
        }

        public override void Initialize(NXDocument document)
        {
            base.Initialize(document);

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
                .OfType<ModelElement>()
                .ToList();

            foreach (var model in models)
            {
                // obtain instances
                var instances = model
                    .Descendants(false)
                    .OfType<InstanceElement>()
                    .ToList();

                // initialize the instances
                foreach (var instance in instances)
                    instance.State.Initialize(model, instance);
            }

            // perform refresh of just loaded visuals
            if (models.All(i => i.State.Ready))
                foreach (var binding in Document.Root.Descendants(true).OfType<BindingElement>())
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

            return (XFormsElement)Activator.CreateInstance(type, new object[] { node });
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

        void Form_ProcessSubmit(object sender, EventArgs e)
        {
            Submit();
        }

        void VersionExceptionEventDefaultAction(VersionExceptionEvent ev)
        {
            System.Console.WriteLine(VersionExceptionEvent.Name);
            Document.Root.GetState<ModuleState>().Failed = true;
        }

        void LinkExceptionEventDefaultAction(LinkExceptionEvent ev)
        {
            System.Console.WriteLine(LinkExceptionEvent.Name);
            Document.Root.GetState<ModuleState>().Failed = true;
        }

        void BindingExceptionEventDefaultAction(BindingExceptionEvent ev)
        {
            System.Console.WriteLine(BindingExceptionEvent.Name);
            Document.Root.GetState<ModuleState>().Failed = true;
        }

        public override bool Invoke()
        {
            if (Document.Root.GetState<ModuleState>().Failed)
                return false;

            var work = false;

            // obtain all model visuals
            var models = Document.Root
                .Descendants(true)
                .OfType<ModelElement>()
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!Document.Root.GetState<ModuleState>().Failed)
                    if (!model.State.Construct)
                    {
                        model.Interface<IEventTarget>().DispatchEvent(new ModelConstructEvent(model).Event);
                        work = true;
                    }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                if (!Document.Root.GetState<ModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.ConstructDone)
                        {
                            model.Interface<IEventTarget>().DispatchEvent(new ModelConstructDoneEvent(model).Event);
                            work = true;
                        }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                if (!Document.Root.GetState<ModuleState>().Failed)
                    foreach (var model in models)
                        if (!model.State.Ready)
                        {
                            model.Interface<IEventTarget>().DispatchEvent(new ReadyEvent(model).Event);
                            work = true;
                        }

            if (Document.Root.GetState<ModuleState>().Failed)
                return work;

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.RebuildFlag))
                    if (!Document.Root.GetState<ModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new RebuildEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RecalculateFlag))
                    if (!Document.Root.GetState<ModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new RecalculateEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RevalidateFlag))
                    if (!Document.Root.GetState<ModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new RevalidateEvent(model).Event);
                    }

                foreach (var model in models.Where(i => i.State.RefreshFlag))
                    if (!Document.Root.GetState<ModuleState>().Failed)
                    {
                        work = true;
                        model.Interface<IEventTarget>().DispatchEvent(new RefreshEvent(model).Event);
                    }
            }

            return work;
        }

        /// <summary>
        /// Loads the instance data associated with the given model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal void ProcessModelInstance(ModelElement model)
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
                        var resource = Document.Container.GetExportedValue<IResolver>().Get(u);
                        if (resource == null)
                            throw new FileNotFoundException("Could not load resource", instanceSrc);

                        // parse resource into new DOM
                        var instanceDataDocument = XDocument.Load(resource);

                        // add to model
                        instance.State.Initialize(model, instance, instanceDataDocument);
                    }
                    catch (UriFormatException)
                    {
                        target.DispatchEvent(new LinkExceptionEvent(model).Event);
                    }
                }
                else if (instanceChildElements.Length >= 2)
                {
                    // invalid number of child elements
                    target.DispatchEvent(new LinkExceptionEvent(model).Event);
                }
                else if (instanceChildElements.Length == 1)
                {
                    instance.State.Initialize(model, instance, new XDocument(instanceChildElements[0]));
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="XPathNavigator"/> for the given model item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        XPathNavigator CreateNavigator(ModelItem item)
        {
            if (item.Xml is XAttribute)
            {
                // navigator needs to be created on parent, and navigated to attribute
                var nav = item.Xml.Parent.CreateNavigator();
                nav.MoveToAttribute(((XAttribute)item.Xml).Name.LocalName, ((XAttribute)item.Xml).Name.NamespaceName);
                return nav;
            }

            if (item.Xml is XElement)
                return ((XElement)item.Xml).CreateNavigator();

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Evaluates the given XPath expression.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evaluationContext"></param>
        /// <param name="expression"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        internal object EvaluateXPath(NXNode node, EvaluationContext evaluationContext, string expression, XPathResultType resultType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(evaluationContext != null);
            Contract.Requires<ArgumentNullException>(expression != null);

            var nc = new XFormsXsltContext(node, evaluationContext);
            var nv = CreateNavigator(evaluationContext.ModelItem);
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
        internal EvaluationContext ResolveInScopeEvaluationContext(XFormsElement visual)
        {
            EvaluationContext ec = null;

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
                    .TakeWhile(i => !(i is Group))
                    .OfType<ModelElement>()
                    .Select(i => i.DefaultEvaluationContext)
                    .FirstOrDefault();

            return ec;
        }

        /// <summary>
        /// Resolves the <see cref="EvaluationContext"/> to be used by the given visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal EvaluationContext ResolveBindingEvaluationContext(XFormsElement visual)
        {
            // attempt to retrieve model state given by 'model' attribute
            var modelAttr = GetAttributeValue(visual.Xml, "model");
            if (!string.IsNullOrWhiteSpace(modelAttr))
            {
                // find referenced model visual
                var model = Document.Root
                    .Descendants(true)
                    .TakeWhile(i => !(i is Group))
                    .OfType<ModelElement>()
                    .SingleOrDefault(i => Document.GetElementId(i.Xml) == modelAttr);

                if (model != null)
                    return model.Context;
                else
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new BindingExceptionEvent(visual).Event);
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
        internal Binding ResolveSingleNodeBinding(BindingElement visual)
        {
            var element = visual.Xml as XElement;
            if (element == null)
                return null;

            // attempt to resolve 'bind' attribute to bind element's context
            var bd = GetAttributeValue(visual.Xml, "bind");
            if (bd != null)
            {
                var bind = (BindElement)visual.ResolveId(bd);

                // invalid bind element
                if (bind == null ||
                    bind.Context == null)
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new BindingExceptionEvent(visual).Event);
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
                return new Binding(visual, ec, xp);
            }

            return null;
        }

        /// <summary>
        /// Resolves the node-set binding on <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal Binding ResolveNodeSetBinding(BindingElement visual)
        {
            // attempt to resolve 'bind' attribute to bind element's context
            var bindAttr = GetAttributeValue(visual.Xml, "bind");
            if (bindAttr != null)
            {
                var bind = (BindElement)visual.ResolveId(bindAttr);

                // invalid bind element
                if (bind == null ||
                    bind.Binding == null)
                {
                    visual.Interface<IEventTarget>().DispatchEvent(new BindingExceptionEvent(visual).Event);
                    return null;
                }

                return bind.Binding;
            }

            var ec = ResolveBindingEvaluationContext(visual);
            if (ec != null)
            {
                var nodesetAttr = GetAttributeValue(visual.Xml, "nodeset");
                if (nodesetAttr != null)
                    return new Binding(visual, ec, nodesetAttr);
            }

            return null;
        }

        /// <summary>
        /// Invokes the given action visual properly.
        /// </summary>
        /// <param name="visual"></param>
        internal void InvokeAction(IActionElement visual)
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

        internal void RaiseMessage(MessageElement visual)
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
                .OfType<SubmissionElement>();

            // raise a submit event for each submission
            foreach (var visual in visuals)
                visual.Interface<IEventTarget>().DispatchEvent(new SubmitEvent(visual).Event);
        }

    }

}
