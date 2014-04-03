using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Represents an element in the document.
    /// </summary>
    [Public]
    public class NXElement :
        NXContainer
    {

        #region Static

        static IEnumerable<XElement> emptySequence;

        /// <summary>
        /// Gets an empty collection of elements.
        /// </summary>
        public static IEnumerable<XElement> EmptySequence
        {
            get { return emptySequence ?? (emptySequence = new XElement[0]); }
        }

        #endregion

        readonly XName name;
        string uniqueId;
        Dictionary<XNode, NXNode> cache = new Dictionary<XNode, NXNode>();
        bool created = false;
        internal LinkedList<NXAttribute> attributes = new LinkedList<NXAttribute>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXElement(XName name)
            : base()
        {
            Contract.Requires<ArgumentNullException>(name != null);

            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NXElement(XElement xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            this.name = xml.Name;

            foreach (var attr in xml.Attributes())
                this.attributes.AddLast(new NXAttribute(attr));
        }

        /// <summary>
        /// Gets or sets the name of this element.
        /// </summary>
        [Public]
        public XName Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets a reference to the underlying DOM element.
        /// </summary>
        public new XElement Xml
        {
            get { return (XElement)base.Xml; }
            protected set { base.Xml = value; }
        }

        /// <summary>
        /// Removes the specified attribute.
        /// </summary>
        /// <param name="attribute"></param>
        public void RemoveAttribute(NXAttribute attribute)
        {
            attributes.Remove(attribute);
        }

        public LinkedList<object> Storage
        {
            get { return Document.NodeState.Get(this); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="NXNode"/> within the current naming scope.
        /// </summary>
        [Public]
        public virtual string Id
        {
            get { return (string)this.Attribute("id"); }
        }

        /// <summary>
        /// Returns a unique identifier for this visual, considering naming scopes.
        /// </summary>
        [Public]
        public string UniqueId
        {
            get { return uniqueId ?? (uniqueId = CreateUniqueId()); }
        }

        /// <summary>
        /// Implements the getter for UniqueId.
        /// </summary>
        /// <returns></returns>
        string CreateUniqueId()
        {
            var namingScope = this.Ancestors()
                .OfType<INamingScope>()
                .FirstOrDefault();

            if (namingScope == null)
                return Id;
            else
                return ((NXElement)namingScope).UniqueId + "_" + Id;
        }

        protected override void OnAdded(NXObjectEventArgs args)
        {
            base.OnAdded(args);

            // on first add to parent, generate children
            if (!created)
            {
                created = true;
                CreateNodes();
            }
        }

        /// <summary>
        /// Creates the container items.
        /// </summary>
        protected virtual void CreateNodes()
        {
            RemoveNodes();

            if (Xml != null)
                foreach (var node in CreateNodesFromXElement(Xml, true))
                    Add(node);
        }

        /// <summary>
        /// Generates the sequence of container item nodes given the source <see cref="XElememt"/>. Optionally includes text content.
        /// </summary>
        /// <param name="xelement"></param>
        /// <param name="includeTextContent"></param>
        /// <returns></returns>
        protected IEnumerable<NXNode> CreateNodesFromXElement(XElement xelement, bool includeTextContent = false)
        {
            var xnodes = xelement.Nodes().ToArray();
            for (int i = 0; i < xnodes.Length; i++)
            {
                var xnode = xnodes[i];

                // if we're ignoring text, skip text nodes
                if (!includeTextContent && xnode is XText)
                    continue;

                // restore existing visual child, if it exists
                var node = cache.GetOrDefault(xnode);
                if (node != null)
                    yield return node;

                // create new child
                node = Document.CreateNode(xnode);
                if (node != null)
                {
                    cache[xnode] = node;
                    yield return node;
                }
            }
        }

        public T GetState<T>()
            where T : class, new()
        {
            Contract.Ensures(Contract.Result<T>() != null);

            var state = Storage.OfType<T>().FirstOrDefault();
            if (state == null)
                Storage.AddLast(state = new T());

            return state;
        }

        /// <summary>
        /// Looks up the closest xmlns declaration for the given prefix that is in scope for the current node and
        /// returns the namespace URI in the declaration.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override string GetNamespaceOfPrefix(string prefix)
        {
            var xmlns = this.Attribute(XNamespace.Xmlns + prefix);
            if (xmlns != null)
                return xmlns.Value;

            return Parent.GetNamespaceOfPrefix(prefix);
        }

        /// <summary>
        /// Looks up the closest xmlns declaration for the given namespace URI that is in scope for the current node
        /// and returns the prefix defined in that declaration.
        /// </summary>
        /// <param name="namespaceURI"></param>
        /// <returns></returns>
        public override string GetPrefixOfNamespace(string namespaceURI)
        {
            var prefix = this.Attributes()
                .Where(i => i.Name.Namespace == XNamespace.Xmlns)
                .Where(i => i.Value == namespaceURI)
                .Select(i => i.Name.LocalName)
                .FirstOrDefault();
            if (prefix != null)
                return prefix;

            return Parent.GetPrefixOfNamespace(namespaceURI);
        }

    }

}
