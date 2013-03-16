using System;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ISIS.Forms.XForms
{

    public class XFormsBinding
    {

        private bool resultCached;
        private object result;
        private bool nodeCached;
        private XObject node;
        private bool nodesCached;
        private XObject[] nodes;
        private bool valueCached;
        private string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal XFormsBinding(IFormProcessor form, XFormsVisual visual, XFormsEvaluationContext ec, string xp)
        {
            Visual = visual;
            Context = ec;
            XPathExpression = xp;
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
        public XFormsVisual Visual { get; private set; }

        /// <summary>
        /// Gets the <see cref="XFormsEvaluationContext"/> used to resolve this binding.
        /// </summary>
        public XFormsEvaluationContext Context { get; private set; }

        /// <summary>
        /// Gets the XPath expression that describes this binding.
        /// </summary>
        public string XPathExpression { get; private set; }

        /// <summary>
        /// Gets the raw result from the binding.
        /// </summary>
        public object Result
        {
            get
            {
                if (!resultCached)
                {
                    try
                    {
                        result = Module.EvaluateXPath(Context, new VisualXmlNamespaceContext(Visual), Visual, XPathExpression, XPathResultType.NodeSet);
                    }
                    catch
                    {
                        result = Module.EvaluateXPath(Context, new VisualXmlNamespaceContext(Visual), Visual, XPathExpression, XPathResultType.Any);
                    }

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
                    if (Result is string)
                        nodes = null;
                    else if (Result is IEnumerable)
                        nodes = ((IEnumerable)Result).OfType<XObject>().ToArray();

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
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Registers an update for the binding's value.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(string newValue)
        {
            if (Node != null)
                Module.SetModelItemValue(Context, Node, newValue);
            else
                throw new InvalidOperationException();
        }

    }

}
