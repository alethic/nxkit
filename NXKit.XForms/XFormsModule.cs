using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms
{

    [Export(typeof(XFormsModule))]
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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        [ImportingConstructor]
        public XFormsModule(NXDocumentHost document)
            : base(document)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            // obtain all model visuals
            var models = Document.Root
                .DescendantsAndSelf(Constants.XForms_1_0 + "model")
                .OfType<XElement>()
                .ToList();

            foreach (var model in models)
            {
                // obtain instances
                var instances = model
                    .Descendants(Constants.XForms_1_0 + "instance")
                    .ToList();

                // initialize the instances
                foreach (var instance in instances)
                    instance.Interface<Instance>().State.Initialize(model, instance);
            }

            //// perform refresh of just loaded visuals
            //if (models.All(i => i.State.Ready))
            //    foreach (var binding in Document.Root.Descendants(true).OfType<BindingElement>())
            //        binding.
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

        public override bool Invoke()
        {
            var work = false;

            // obtain all model visuals
            var models = Document.Root
                .Descendants(Constants.XForms_1_0 + "model")
                .Select(i => i.Interface<Model>())
                .ToList();

            // raise construct event on all non-constructed models
            foreach (var model in models)
                if (!model.State.Construct)
                {
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.ModelConstruct);
                    work = true;
                }

            // if all models have passed construct, raise construct done event
            if (models.All(i => i.State.Construct))
                foreach (var model in models)
                    if (!model.State.ConstructDone)
                    {
                        model.Element.Interface<INXEventTarget>().DispatchEvent(Events.ModelConstructDone);
                        work = true;
                    }

            // if all models have passed construct-done, raise ready event
            if (models.All(i => i.State.ConstructDone))
                foreach (var model in models)
                    if (!model.State.Ready)
                    {
                        model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Ready);
                        work = true;
                    }

            // only process main events if all models are ready
            if (models.All(i => i.State.Ready))
            {
                foreach (var model in models.Where(i => i.State.RebuildFlag))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Rebuild);
                }

                foreach (var model in models.Where(i => i.State.RecalculateFlag))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Recalculate);
                }

                foreach (var model in models.Where(i => i.State.RevalidateFlag))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Revalidate);
                }

                foreach (var model in models.Where(i => i.State.RefreshFlag))
                {
                    work = true;
                    model.Element.Interface<INXEventTarget>().DispatchEvent(Events.Refresh);
                }
            }

            return work;
        }

        /// <summary>
        /// Loads the instance data associated with the given model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal void ProcessModelInstance(Model model)
        {
            var target = model.Element.Interface<INXEventTarget>();
            if (target == null)
                throw new NullReferenceException();

            foreach (var instance in model.Instances)
            {
                // extract instance values from xml
                var instanceSrc = GetAttributeValue(instance.Element, "src");
                var instanceChildElements = instance.Element.Elements().ToArray();

                if (!string.IsNullOrWhiteSpace(instanceSrc))
                {
                    try
                    {
                        // normalize uri with base
                        var u = new Uri(instanceSrc, UriKind.RelativeOrAbsolute);
                        if (instance.Element.Xml.BaseUri.TrimToNull() != null && !u.IsAbsoluteUri)
                            u = new Uri(new Uri(instance.Element.Xml.BaseUri), u);

                        // return resource as a stream
                        var request = WebRequest.Create(u);
                        request.Method = "GET";
                        var response = request.GetResponse().GetResponseStream();
                        if (response == null)
                            throw new FileNotFoundException("Could not load resource", instanceSrc);

                        // parse resource into new DOM
                        var instanceDataDocument = XDocument.Load(response);

                        // add to model
                        instance.State.Initialize(model.Element, instance.Element, instanceDataDocument);
                    }
                    catch (UriFormatException)
                    {
                        target.DispatchEvent(Events.Rebuild);
                    }
                }
                else if (instanceChildElements.Length >= 2)
                {
                    // invalid number of child elements
                    target.DispatchEvent(Events.LinkException);
                }
                else if (instanceChildElements.Length == 1)
                {
                    instance.State.Initialize(model.Element, instance.Element, new XDocument(instanceChildElements[0]));
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
        internal object EvaluateXPath(XNode node, EvaluationContext evaluationContext, string expression, XPathResultType resultType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Host() != null);
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
        /// Resolves the evaluation context inherited from parents of <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal EvaluationContext ResolveInScopeEvaluationContext(XElement element)
        {
            EvaluationContext ec = null;

            // search up visual tree for initial context
            if (ec == null)
                ec = element
                    .Ancestors()
                    .SelectMany(i => i.Interfaces<IEvaluationContextScope>())
                    .Select(i => i.Context)
                    .FirstOrDefault(i => i != null);

            // default to default model
            if (ec == null)
                ec = Document.Root
                    .DescendantsAndSelf(Constants.XForms_1_0 + "model")
                    .SelectMany(i => i.Interfaces<Model>())
                    .Select(i => i.DefaultEvaluationContext)
                    .FirstOrDefault();

            return ec;
        }

        /// <summary>
        /// Invokes the given action visual properly.
        /// </summary>
        /// <param name="action"></param>
        internal void InvokeAction(IAction action)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            var outermostAction = !executingOutermostActionHandler;
            if (outermostAction)
                executingOutermostActionHandler = true;

            action.Invoke();

            if (outermostAction)
            {
                executingOutermostActionHandler = false;
                Invoke();
            }
        }

    }

}
