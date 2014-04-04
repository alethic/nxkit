using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Represents a node or an attribute in the document tree.
    /// </summary>
    public abstract class NXObject
    {

        internal NXContainer parent;
        LinkedList<object> annotations = new LinkedList<object>();

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
            Contract.Requires<ArgumentNullException>(args != null);

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
            Contract.Requires<ArgumentNullException>(args != null);

            if (Removed != null)
                Removed(this, args);
        }

        /// <summary>
        /// Gets the annotations of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<object> Annotations(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return annotations.Where(i => type.IsInstanceOfType(i));
        }

        /// <summary>
        /// Gets the annotations of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Annotations<T>()
        {
            return annotations.OfType<T>();
        }

        /// <summary>
        /// Gets the first annotation of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Annotation(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return Annotations(type).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first annotation of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Annotation<T>()
        {
            return Annotations<T>().FirstOrDefault();
        }

        /// <summary>
        /// Adds a new annotation.
        /// </summary>
        /// <param name="annotation"></param>
        public void AddAnnotation(object annotation)
        {
            Contract.Requires<ArgumentNullException>(annotation != null);

            annotations.AddLast(annotation);
        }

        /// <summary>
        /// Removes the annotations of the specified type.
        /// </summary>
        /// <param name="type"></param>
        public void RemoveAnnotations(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            var nodes = annotations.Forwards()
                .Where(i => type.IsInstanceOfType(i.Value))
                .ToList();
            foreach (var node in nodes)
                node.List.Remove(node);
        }

        /// <summary>
        /// Removes the annotations of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveAnnotations<T>()
        {
            var nodes = annotations.Forwards()
                .Where(i => i.Value is T)
                .ToList();
            foreach (var node in nodes)
                node.List.Remove(node);
        }

    }

}
