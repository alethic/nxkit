using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsultes an XPath binding expression configured on a <see cref="XNode"/>.
    /// </summary>
    public class Binding
    {

        static readonly ModelItem[] EmptyModelItemSequence = new ModelItem[0];

        readonly XElement element;
        readonly EvaluationContext context;
        readonly string xpath;

        Lazy<object> result;
        Lazy<ModelItem[]> modelItems;
        Lazy<ModelItem> modelItem;
        Lazy<string> value;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(result != null);
            Contract.Invariant(modelItems != null);
            Contract.Invariant(modelItem != null);
            Contract.Invariant(value != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <param name="xpath"></param>
        internal Binding(XElement element, EvaluationContext context, string xpath)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Host() != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(xpath != null);

            this.element = element;
            this.context = context;
            this.xpath = xpath;

            // initial load of values
            Recalculate();
        }

        /// <summary>
        /// <see cref="Element"/> to which this binding is related.
        /// </summary>
        public XElement Element
        {
            get { return element; }
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
        public string XPathExpression
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
            return Context.EvaluateXPath(Element, XPathExpression, XPathResultType.NodeSet);
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
                    .Select(i => i.AnnotationOrCreate<ModelItem>(() => new ModelItem(i)))
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
