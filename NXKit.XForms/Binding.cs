using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsultes an XPath binding expression configured on a <see cref="XNode"/>.
    /// </summary>
    public class Binding
    {

        readonly XNode node;
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
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <param name="xpath"></param>
        internal Binding(XNode node, EvaluationContext context, string xpath)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Host() != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(xpath != null);

            this.node = node;
            this.context = context;
            this.xpath = xpath;

            // initial load of values
            Refresh();
        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/>.
        /// </summary>
        XFormsModule Module
        {
            get { return Node.Host().Module<XFormsModule>(); }
        }

        /// <summary>
        /// <see cref="Node"/> to which this binding is related.
        /// </summary>
        public XNode Node
        {
            get { return node; }
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
            return Module.EvaluateXPath(Node, Context, XPathExpression, XPathResultType.NodeSet);
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
                    .Select(i => i.UnderlyingObject)
                    .Cast<XObject>()
                    .Select(i => new ModelItem(Module, i))
                    .ToArray();
            else
                return null;
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
        public void Refresh()
        {
            result = new Lazy<object>(() => GetResult());
            modelItems = new Lazy<ModelItem[]>(() => GetModelItems());
            modelItem = new Lazy<ModelItem>(() => GetModelItem());
            value = new Lazy<string>(() => GetValue());
        }

    }

}
