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
    public abstract class NXElement :
        NXContainer
    {

        string uniqueId;
        Dictionary<XNode, NXNode> cache = new Dictionary<XNode, NXNode>();
        bool created = false;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXElement()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NXElement(XElement element)
            : base(element)
        {

        }

        /// <summary>
        /// Gets a reference to the underlying DOM element.
        /// </summary>
        public new XElement Xml
        {
            get { return (XElement)base.Xml; }
            protected set { base.Xml = value; }
        }

        public LinkedList<object> Storage
        {
            get { return Document.NodeState.Get(this); }
        }

        /// <summary>
        /// Unique identifier for the <see cref="NXNode"/> within the current naming scope.
        /// </summary>
        [Interactive]
        public abstract string Id { get; }

        /// <summary>
        /// Returns a unique identifier for this visual, considering naming scopes.
        /// </summary>
        [Interactive]
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

    }

}
