using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Represents a node in the document tree.
    /// </summary>
    public abstract class NXNode :
        NXObject
    {

        XNode xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXNode()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXNode(XNode xml)
            : base()
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            this.xml = xml;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXNode(NXContainer parent)
            : base(parent)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xml"></param>
        protected NXNode(NXContainer parent, XNode xml)
            : this(parent)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
            Contract.Requires<ArgumentNullException>(xml != null);

            this.xml = xml;
        }

        /// <summary>
        /// Gets a reference to the underlying XML node.
        /// </summary>
        public XNode Xml
        {
            get { return xml; }
            protected set { xml = value; }
        }

        /// <summary>
        /// Removes this node from its parent.
        /// </summary>
        /// <param name="node"></param>
        public void Remove()
        {
            var old = Parent;
            if (old == null)
                throw new InvalidOperationException();

            // raise events and remove node
            old.OnChanging(new NXObjectChangeEventArgs(this, NXObjectChange.Remove));
            old.nodes.Remove(this);
            Parent = null;
            old.OnChanged(new NXObjectChangeEventArgs(this, NXObjectChange.Remove));
        }

        protected override void OnAdded(NXObjectEventArgs args)
        {
            base.OnAdded(args);

            // enumerate all interfaces to ensure initialization
            this.Interfaces().ToList();
        }

        #region Naming Scope

        /// <summary>
        /// Finds the <see cref="NXNode"/> with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NXElement ResolveId(string id)
        {
            // discover the root visual
            var root = this.Ancestors()
                .Prepend(this as NXElement)
                .First(i => i != null && i.Parent == null);

            // naming scope of current visual
            var namingScopes = this.Ancestors()
                .OfType<INamingScope>()
                .Append(null)
                .ToArray();

            // search all descendents of the root element that are sharing naming scopes with myself
            foreach (var visual in root.DescendantsIncludeNS(namingScopes).OfType<NXElement>())
                if (visual.Id == id)
                    return visual;

            return null;
        }

        /// <summary>
        /// Gets the naming scope of <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected INamingScope GetNamingScope(NXNode visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return visual.Ancestors()
                .OfType<INamingScope>()
                .FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// Returns a string representation of this <see cref="NXNode"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name;
        }

    }

}
