using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a node that can contain other nodes.
    /// </summary>
    public abstract class NXContainer :
        NXNode
    {

        internal object content;

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
        public NXContainer(NXElement parent)
            : base(parent)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXContainer(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xml"></param>
        protected NXContainer(NXElement parent, XContainer xml)
            : base(parent, xml)
        {

        }

        /// <summary>
        /// Gets a reference to the underlying DOM element.
        /// </summary>
        public new XContainer Xml
        {
            get { return (XContainer)base.Xml; }
            internal set { base.Xml = value; }
        }

        public NXNode FirstNode
        {
            get 
            { 
                NXNode lastNode = this.LastNode;
                if (lastNode == null)
                {
                    return null;
                }
                return lastNode.next;
            }
        }

        public NXNode LastNode
        {
            get
            {
                if (content == null)
                    return null;

                var node = content as NXNode;
                if (node != null)
                    return node;

                var str = content as string;
                if (str != null)
                {
                    if (str.Length == 0)
                        return null;

                    var xText = new NXText(str)
                    {
                        parent = this
                    };
                    xText.next = xText;

                    Interlocked.CompareExchange<object>(ref this.content, xText, str);
                }

                return (NXNode)content;
            }
        }

        /// <summary>
        /// Gets the children <see cref="NXNode"/>s of this <see cref="NXContainer"/>.
        /// </summary>
        public IEnumerable<NXNode> Nodes()
        {
            return nodes;
        }

        /// <summary>
        /// Gets the children <see cref="NXElements"/> of this <see cref="NXContainer"/>.
        /// </summary>
        public IEnumerable<NXElement> Elements()
        {
            return nodes.OfType<NXElement>();
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
            if (this is NXElement)
                node.Parent = (NXElement)this;
        }

        internal void AddNode(NXNode n)
        {
           // this.ValidateNode(n, this);
            if (n.parent == null)
            {
                NXNode xNode = this;
                while (xNode.parent != null)
                {
                    xNode = xNode.parent;
                }
                if (n == xNode)
                {
                    n = n.CloneNode();
                }
            }
            else
            {
                n = n.CloneNode();
            }
            this.ConvertTextToNode();
            this.AppendNode(n);
        }

        /// <summary>
        /// Removes the child nodes from this container.
        /// </summary>
        public void RemoveNodes()
        {
            foreach (var node in Nodes().Reverse().ToArray())
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

            foreach (var container in Nodes())
                if (container is NXContainer)
                    foreach (var descendant in ((NXContainer)container).DescendantsIncludeNS(ns))
                        yield return descendant;
                else
                    yield return container;
        }

    }

}
