using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a node that can contain other nodes.
    /// </summary>
    public abstract class NXContainer :
        NXNode
    {

        internal LinkedList<NXNode> nodes =
            new LinkedList<NXNode>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXContainer()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXContainer(NXContainer parent)
            : base(parent)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXContainer(XContainer container)
            : base(container)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="xelement"></param>
        protected NXContainer(NXContainer parent, XElement xelement)
            : base(parent, xelement)
        {

        }

        /// <summary>
        /// Gets a reference to the underlying DOM element.
        /// </summary>
        public XContainer Xml
        {
            get { return (XContainer)base.Xml; }
            internal set { base.Xml = value; }
        }

        /// <summary>
        /// Gets the children <see cref="NXNode"/>s of this <see cref="NXContainer"/>.
        /// </summary>
        public IEnumerable<NXNode> Nodes
        {
            get { return nodes; }
        }

        /// <summary>
        /// Gets the children <see cref="NXElements"/> of this <see cref="NXContainer"/>.
        /// </summary>
        public IEnumerable<NXElement> Elements
        {
            get { return nodes.OfType<NXElement>(); }
        }

        /// <summary>
        /// Adds the given node to this element.
        /// </summary>
        /// <param name="node"></param>
        public void Add(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            OnChanging(new NXObjectChangeEventArgs(node, NXObjectChange.Add));
            nodes.AddLast(node);
            node.Parent = this;
            OnChanged(new NXObjectChangeEventArgs(node, NXObjectChange.Add));
        }

        /// <summary>
        /// Removes the child nodes from this container.
        /// </summary>
        public void RemoveNodes()
        {
            foreach (var node in Nodes.Reverse().ToArray())
                node.Remove();
        }

        public event NXObjectChangeEventHandler Changing;

        /// <summary>
        /// Configures an object as a child.
        /// </summary>
        /// <param name="obj"></param>
        internal void OnChanging(NXObjectChangeEventArgs args)
        {
            if (Changing != null)
                Changing(this, args);
        }

        public event NXObjectChangeEventHandler Changed;

        /// <summary>
        /// Configures an object as a child.
        /// </summary>
        /// <param name="obj"></param>
        internal void OnChanged(NXObjectChangeEventArgs args)
        {
            if (Changed != null)
                Changed(this, args);
        }

        internal IEnumerable<NXNode> DescendantsIncludeNS(INamingScope[] ns)
        {
            if (!ns.Contains(GetNamingScope(this)))
                yield break;

            yield return this;

            foreach (var container in Nodes)
                if (container is NXContainer)
                    foreach (var descendant in ((NXContainer)container).DescendantsIncludeNS(ns))
                        yield return descendant;
                else
                    yield return container;
        }

    }

}
