using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NXKit.Diagnostics;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsultes an XPath binding expression configured on a <see cref="XNode"/>.
    /// </summary>
    public class Binding
    {

        static readonly ModelItem[] EmptyModelItemSequence = new ModelItem[0];

        readonly XObject xml;
        readonly EvaluationContext context;
        readonly XPathExpression xpath;
        readonly ITraceService trace;
        Lazy<object> result;
        Lazy<ModelItem[]> modelItems;
        Lazy<ModelItem> modelItem;
        Lazy<string> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="context"></param>
        /// <param name="xpath"></param>
        /// <param name="trace"></param>
        internal Binding(XObject xml, EvaluationContext context, XPathExpression xpath, ITraceService trace)
        {
            this.xml = xml ?? throw new ArgumentNullException(nameof(xml));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.xpath = xpath ?? throw new ArgumentNullException(nameof(xpath));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            // initial load of values
            Recalculate();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="context"></param>
        /// <param name="xpath"></param>
        /// <param name="trace"></param>
        internal Binding(XObject xml, EvaluationContext context, string xpath, ITraceService trace)
            : this(xml, context, context.CompileXPath(xml, xpath), trace)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (xpath == null)
                throw new ArgumentNullException(nameof(xpath));
            if (trace is null)
                throw new ArgumentNullException(nameof(trace));
        }

        /// <summary>
        /// <see cref="XElement"/> to which this binding is related.
        /// </summary>
        public XObject Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> used to resolve this binding.
        /// </summary>
        public EvaluationContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// Gets the XPath expression that describes this binding.
        /// </summary>
        public XPathExpression XPathExpression
        {
            get { return xpath; }
        }

        /// <summary>
        /// Gets the raw result from the binding.
        /// </summary>
        public object Result
        {
            get { return result.Value; }
        }

        object GetResult()
        {
            return Context.EvaluateXPath(Xml, XPathExpression, XPathResultType.NodeSet);
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public ModelItem[] ModelItems
        {
            get { return modelItems.Value; }
        }

        ModelItem[] GetModelItems()
        {
            if (Result is XPathNodeIterator)
                return ((XPathNodeIterator)Result)
                    .Cast<XPathNavigator>()
                    .Select(i => (XObject)i.UnderlyingObject)
                    .Select(i => ModelItem.Get(i, trace))
                    .ToArray();
            else
                return EmptyModelItemSequence;
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public ModelItem ModelItem
        {
            get { return modelItem.Value; }
        }

        ModelItem GetModelItem()
        {
            return ModelItems.FirstOrDefault();
        }

        /// <summary>
        /// Gets the bound simple content, if applicable.
        /// </summary>
        public string Value
        {
            get { return value.Value; }
        }

        string GetValue()
        {
            if (ModelItem != null &&
                ModelItem.Value != null)
                return ModelItem.Value;
            else if (Result is string)
                return (string)Result;
            else if (Result is int)
                return XmlConvert.ToString((int)Result);
            else if (Result is double)
                return XmlConvert.ToString((double)Result);
            else if (Result is bool)
                return XmlConvert.ToString((bool)Result);
            return null;
        }

        /// <summary>
        /// Initiates a refresh of the binding.
        /// </summary>
        public void Recalculate()
        {
            result = new Lazy<object>(() => GetResult());
            modelItems = new Lazy<ModelItem[]>(() => GetModelItems());
            modelItem = new Lazy<ModelItem>(() => GetModelItem());
            value = new Lazy<string>(() => GetValue());
        }

    }

}
