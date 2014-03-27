using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Represents a node or an attribute in the document tree.
    /// </summary>
    public abstract class NXObject
    {

        NXContainer parent;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        public NXObject()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public NXObject(NXContainer parent)
            : this()
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
        }

        /// <summary>
        /// Gets the <see cref="INXDocument"/>
        /// </summary>
        public virtual NXDocument Document
        {
            get { return Parent != null ? Parent.Document : null; }
        }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        public NXContainer Parent
        {
            get { return parent; }
            internal set { SetParent(value); }
        }

        /// <summary>
        /// Implements the setter for Parent.
        /// </summary>
        /// <param name="container"></param>
        void SetParent(NXContainer container)
        {
            if (parent == container)
                return;

            // set new parent
            var old = parent;
            parent = container;

            // resulted in removal
            if (old != null && parent == null)
                OnRemoved(new NXObjectEventArgs(this));

            // resulted in adding
            if (old == null && parent != null)
                OnAdded(new NXObjectEventArgs(this));
        }

        /// <summary>
        /// raised when this <see cref="NXNode"/> is added to a container.
        /// </summary>
        public event NXObjectEventHandler Added;

        /// <summary>
        /// Raise the OnAdded event.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnAdded(NXObjectEventArgs args)
        {
            if (Added != null)
                Added(this, args);
        }

        /// <summary>
        /// Raised when this <see cref="NXNode"/> is removed from a container.
        /// </summary>
        public event NXObjectEventHandler Removed;

        /// <summary>
        /// Raises the Removed event.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnRemoved(NXObjectEventArgs args)
        {
            if (Removed != null)
                Removed(this, args);
        }

    }

}
