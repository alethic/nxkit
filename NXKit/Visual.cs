using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Represents a node in the visual tree.
    /// </summary>
    public abstract class Visual
    {

        bool addedEventRaised = false;
        LinkedList<object> storage;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Visual()
        {
            this.storage = new LinkedList<object>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        protected Visual(INXDocument document, ContentVisual parent, XNode node)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            Initialize(document, parent, node);
        }

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        public void Initialize(INXDocument document, ContentVisual parent, XNode node)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            Document = document;
            Parent = parent;
            Node = node;
        }

        /// <summary>
        /// <see cref="INXDocument"/> responsible for the visual tree this visual resides within.
        /// </summary>
        public INXDocument Document { get; private set; }

        /// <summary>
        /// Parent <see cref="Visual"/>.
        /// </summary>
        public ContentVisual Parent { get; private set; }

        /// <summary>
        /// Gets a reference to the underlying DOM node.
        /// </summary>
        public XNode Node { get; private set; }

        /// <summary>
        /// Invoked when the visual is added.
        /// </summary>
        public event VisualEventHandler VisualAdded;

        /// <summary>
        /// Raises the VisualAdded event.
        /// </summary>
        void OnVisualAdded()
        {
            if (VisualAdded != null)
                VisualAdded(this, new VisualEventArgs(this));
        }

        /// <summary>
        /// Invoke once per control in refresh to raise the created event.
        /// </summary>
        internal void RaiseVisualAdded()
        {
            if (!addedEventRaised)
            {
                addedEventRaised = true;
                OnVisualAdded();
            }
        }

        /// <summary>
        /// Gets the private storage for this visual.
        /// </summary>
        public LinkedList<object> Storage
        {
            get { return storage; }
        }

        /// <summary>
        /// Gets all the available interfaces.
        /// </summary>
        public IEnumerable<object> Interfaces
        {
            get { return Document.Modules.SelectMany(i => i.GetInterfaces(this)); }
        }

        /// <summary>
        /// Gets the implemented interface specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Interface<T>()
        {
            return Interfaces.OfType<T>().FirstOrDefault();
        }

        #region Navigation

        /// <summary>
        /// Yields each ascendant visual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentVisual> Ascendants()
        {
            var node = this.Parent;
            while (node != null)
            {
                yield return node;
                node = node.Parent;
            }
        }

        #endregion

        #region Naming Scope

        /// <summary>
        /// Finds the <see cref="Visual"/> with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Visual ResolveId(string id)
        {
            // discover the root visual
            var rootVisual = Ascendants()
                .Prepend(this as ContentVisual)
                .First(i => i != null && i.Parent == null);

            // naming scope of current visual
            var namingScopes = Ascendants()
                .OfType<INamingScope>()
                .Append(null)
                .ToArray();

            // search all descendents of the root element that are sharing naming scopes with myself
            foreach (var visual in rootVisual.DescendantsIncludeNS(namingScopes).OfType<ContentVisual>())
                if (visual.Id == id)
                    return visual;

            return null;
        }

        /// <summary>
        /// Gets the naming scope of <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected INamingScope GetNamingScope(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return visual.Ascendants()
                .OfType<INamingScope>()
                .FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// Returns a string representation of this <see cref="Visual"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name;
        }

    }

}
