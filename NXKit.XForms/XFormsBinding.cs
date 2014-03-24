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
    public class XFormsBinding
    {

        readonly XFormsVisual visual;
        readonly XFormsEvaluationContext context;
        readonly string xpath;

        bool resultCached;
        object result;
        bool nodeCached;
        XObject node;
        bool nodesCached;
        XObject[] nodes;
        bool valueCached;
        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="ec">Context in which to begin evaluation</param>
        /// <param name="xp"></param>
        internal XFormsBinding(XFormsVisual visual, XFormsEvaluationContext ec, string xp)
        {
            this.visual = visual;
            this.context = ec;
            this.xpath = xp;
        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/>.
        /// </summary>
        public XFormsModule Module
        {
            get { return Visual.Module; }
        }

        /// <summary>
        /// <see cref="Visual"/> to which this binding is related.
        /// </summary>
        public XFormsVisual Visual
        {
            get { return visual; }
        }

        /// <summary>
        /// Gets the <see cref="XFormsEvaluationContext"/> used to resolve this binding.
        /// </summary>
        public XFormsEvaluationContext Context
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
                    result = Module.EvaluateXPath(Visual, Context, XPathExpression, XPathResultType.NodeSet);
                    resultCached = true;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public XObject Node
        {
            get
            {
                if (!nodeCached)
                {
                    if (Nodes is XObject[])
                        node = Nodes.FirstOrDefault();
                    else if (Result is XNode)
                        node = (XNode)Result;
                    else if (Result is XAttribute)
                        node = (XAttribute)Result;

                    nodeCached = true;
                }

                return node;
            }
        }

        /// <summary>
        /// Gets the unique id of the 
        /// </summary>
        public string NodeUniqueId
        {
            get
            {
                if (Node != null)
                    return Module.GetModelItemUniqueId(Context, Node);

                return null;
            }
        }

        /// <summary>
        /// Gets the bound node, if applicable; otherwise, <c>null</c>.
        /// </summary>
        public XObject[] Nodes
        {
            get
            {
                if (!nodesCached)
                {
                    if (Result is XPathNodeIterator)
                        nodes = ((XPathNodeIterator)Result)
                            .Cast<XPathNavigator>()
                            .Select(i => i.UnderlyingObject)
                            .Cast<XObject>()
                            .ToArray();

                    nodesCached = true;
                }

                return nodes;
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

                    if (Node != null)
                    {
                        if (Node is XText)
                            // node is a simple text node
                            return ((XText)Node).Value;
                        else if (Node is XElement && !((XElement)Node).HasElements)
                        {
                            // node is an element, that contains only text
                            value = ((XElement)Node).Value ?? "";
                        }
                        else if (Node is XAttribute)
                            return ((XAttribute)Node).Value ?? "";
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
        public XName Type
        {
            get { return Node != null ? Module.GetModelItemType(Node) : null; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to relevant data.
        /// </summary>
        public bool Relevant
        {
            get { return Node != null ? Module.GetModelItemRelevant(Node) : true; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to required data.
        /// </summary>
        public bool Required
        {
            get { return Node != null ? Module.GetModelItemRequired(Node) : false; }
        }

        /// <summary>
        /// Gets whether or not this binding is bound to read-only data.
        /// </summary>
        public bool ReadOnly
        {
            get { return Node != null ? Module.GetModelItemReadOnly(Node) : true; }
        }

        /// <summary>
        /// Gest whether or not this binding is bound to valid data.
        /// </summary>
        public bool Valid
        {
            get { return Node != null ? Module.GetModelItemValid(Node) : false; }
        }

        /// <summary>
        /// Registers an update for the binding's node contents.
        /// </summary>
        /// <param name="newElement"></param>
        public void SetValue(XElement newElement)
        {
            if (Node != null && Node is XElement)
                Module.SetModelItemElement(Context, Node, newElement);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Registers an unsetting of the binding's node contents.
        /// </summary>
        public void ClearNode()
        {
            if (Node != null)
                Module.ClearModelItem(Context, Node);
        }

        /// <summary>
        /// Registers an update for the binding's value.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(string newValue)
        {
            if (Node != null)
                Module.SetModelItemValue(Context, Node, newValue);
        }

    }

}
