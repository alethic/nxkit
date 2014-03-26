using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Manages a binding to a model item.
    /// </summary>
    public class Binding
    {

        readonly NXNode node;
        readonly EvaluationContext context;
        readonly string xpath;

        bool resultCached;
        object result;
        bool modelItemCached;
        XObject modelItem;
        bool modelItemsCached;
        XObject[] modelItems;
        bool valueCached;
        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ec">Context in which to begin evaluation</param>
        /// <param name="xp"></param>
        internal Binding(NXNode node, EvaluationContext ec, string xp)
        {
            this.node = node;
            this.context = ec;
            this.xpath = xp;
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
        public XObject ModelItem
        {
            get
            {
                if (!modelItemCached)
                {
                    if (ModelItems is XObject[])
                        modelItem = ModelItems.FirstOrDefault();
                    else if (Result is XNode)
                        modelItem = (XNode)Result;
                    else if (Result is XAttribute)
                        modelItem = (XAttribute)Result;

                    modelItemCached = true;
                }

                return modelItem;
            }
        }

        /// <summary>
        /// Gets the unique id of the 
        /// </summary>
        public string ModelItemUniqueId
        {
            get
            {
                if (ModelItem != null)
                    return Module.GetModelItemUniqueId(Context, ModelItem);

                return null;
            }
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public XObject[] ModelItems
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
                            .ToArray();

                    modelItemsCached = true;
                }

                return modelItems;
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
                    {
                        if (ModelItem is XText)
                            // node is a simple text node
                            return ((XText)ModelItem).Value;
                        else if (ModelItem is XElement && !((XElement)ModelItem).HasElements)
                        {
                            // node is an element, that contains only text
                            value = ((XElement)ModelItem).Value ?? "";
                        }
                        else if (ModelItem is XAttribute)
                            return ((XAttribute)ModelItem).Value ?? "";
                    }
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
        /// Gets the type of the bound data.
        /// </summary>
        public XName ModelItemType
        {
            get { return ModelItem != null ? Module.GetModelItemType(ModelItem) : null; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to relevant data.
        /// </summary>
        public bool ModelItemRelevant
        {
            get { return ModelItem != null ? Module.GetModelItemRelevant(ModelItem) : true; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to required data.
        /// </summary>
        public bool ModelItemRequired
        {
            get { return ModelItem != null ? Module.GetModelItemRequired(ModelItem) : false; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to read-only data.
        /// </summary>
        public bool ModelItemReadOnly
        {
            get { return ModelItem != null ? Module.GetModelItemReadOnly(ModelItem) : true; }
        }

        /// <summary>
        /// Gest whether or not this binding is bound to valid data.
        /// </summary>
        public bool ModelItemValid
        {
            get { return ModelItem != null ? Module.GetModelItemValid(ModelItem) : false; }
        }

        /// <summary>
        /// Registers an update for the binding's node contents.
        /// </summary>
        /// <param name="newElement"></param>
        public void SetValue(XElement newElement)
        {
            if (ModelItem != null && ModelItem is XElement)
                Module.SetModelItemElement(Context, ModelItem, newElement);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Registers an unsetting of the binding's node contents.
        /// </summary>
        public void ClearModelItem()
        {
            if (ModelItem != null)
                Module.ClearModelItem(Context, ModelItem);
        }

        /// <summary>
        /// Registers an update for the binding's value.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(string newValue)
        {
            if (ModelItem != null)
                Module.SetModelItemValue(Context, ModelItem, newValue);
        }

        /// <summary>
        /// Invalidates the binding so that it is recalculated.
        /// </summary>
        public void Invalidate()
        {
            resultCached = modelItemCached = modelItemsCached = valueCached = false;
        }

    }

}
