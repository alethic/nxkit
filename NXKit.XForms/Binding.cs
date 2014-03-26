using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Encapsultes an XPath binding expression configured on a <see cref="NXNode"/>.
    /// </summary>
    public class Binding
    {

        readonly NXNode node;
        readonly EvaluationContext context;
        readonly string xpath;

        bool resultCached;
        object result;
        bool modelItemCached;
        ModelItem modelItem;
        bool modelItemsCached;
        ModelItem[] modelItems;
        string value;
        bool valueCached;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <param name="xpath"></param>
        internal Binding(NXNode node, EvaluationContext context, string xpath)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Requires<ArgumentNullException>(xpath != null);

            this.node = node;
            this.context = context;
            this.xpath = xpath;
        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/>.
        /// </summary>
        XFormsModule Module
        {
            get { return Node.Document.Module<XFormsModule>(); }
        }

        /// <summary>
        /// <see cref="Node"/> to which this binding is related.
        /// </summary>
        public NXNode Node
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
            get
            {
                if (!resultCached)
                {
                    result = Module.EvaluateXPath(Node, Context, XPathExpression, XPathResultType.NodeSet);
                    resultCached = true;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public ModelItem[] ModelItems
        {
            get
            {
                if (!modelItemsCached)
                {
                    if (Result is XPathNodeIterator)
                        modelItems = ((XPathNodeIterator)Result)
                            .Cast<XPathNavigator>()
                            .Select(i => i.UnderlyingObject)
                            .Cast<XObject>()
                            .Select(i => new ModelItem(Module, i))
                            .ToArray();

                    modelItemsCached = true;
                }

                return modelItems;
            }
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public ModelItem ModelItem
        {
            get
            {
                if (!modelItemCached)
                {
                    if (ModelItems is ModelItem[])
                        modelItem = ModelItems.FirstOrDefault();

                    modelItemCached = true;
                }

                return modelItem;
            }
        }

        /// <summary>
        /// Gets the bound simple content, if applicable.
        /// </summary>
        public string Value
        {
            get
            {
                if (!valueCached)
                {
                    value = null;

                    if (ModelItem != null)
                        value = ModelItem.Value;
                    else if (Result is string)
                        value = (string)Result;
                    else if (Result is int)
                        value = XmlConvert.ToString((int)Result);
                    else if (Result is double)
                        value = XmlConvert.ToString((double)Result);
                    else if (Result is bool)
                        value = XmlConvert.ToString((bool)Result);

                    valueCached = true;
                }

                return value;
            }
        }

        /// <summary>
        /// Initiates a refresh of the binding.
        /// </summary>
        public void Refresh()
        {
            resultCached = modelItemsCached = modelItemCached = valueCached = false;
        }

    }

}
