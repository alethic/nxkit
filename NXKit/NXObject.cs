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

        readonly LinkedList<object> storage;
        NXContainer parent;

        /// <summary>
        /// Initialies a new instance.
        /// </summary>
        public NXObject()
        {
            this.storage = new LinkedList<object>();
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
        /// Gets a reference to private data storage on the object.
        /// </summary>
        public LinkedList<object> Storage
        {
            get { return storage; }
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
            internal set { parent = value; }
        }

    }

}
