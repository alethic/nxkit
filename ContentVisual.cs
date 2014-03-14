using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Represents a content node in the visual tree.
    /// </summary>
    public abstract class ContentVisual : 
        Visual
    {

        string uniqueId;
        Visual[] children;
        Dictionary<XNode, Visual> nodeChildren = new Dictionary<XNode, Visual>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ContentVisual()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        protected ContentVisual(INXDocument document, ContentVisual parent, XNode node)
            : base(document, parent, node)
        {
            Contract.Requires<ArgumentNullException>(document != null);
        }

        /// <summary>
        /// Gets a reference to the underlying DOM element.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)Node; }
        }

        /// <summary>
        /// Unique identifier for the <see cref="Visual"/> within the current naming scope.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Returns a unique identifier for this visual, considering naming scopes.
        /// </summary>
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
            var namingScope = Ascendants()
                .OfType<INamingScope>()
                .FirstOrDefault();

            if (namingScope == null)
                return Id;
            else
                return ((ContentVisual)namingScope).UniqueId + "_" + Id;
        }

        /// <summary>
        /// Gets the set of children <see cref="Visual"/>s underneath this <see cref="ContentVisual"/>.
        /// </summary>
        public IEnumerable<Visual> Visuals
        {
            get { return children ?? (children = CreateVisuals().ToArray()); }
        }

        /// <summary>
        /// Creates children <see cref="Visual"/>s. Override to alter the child tree. Default implementation builds
        /// new <see cref="Visual"/> instances from existing children nodes by invoking CreateNodeChildren.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Visual> CreateVisuals()
        {
            return CreateElementVisuals(Element);
        }

        /// <summary>
        /// Creates visual nodes from existing children nodes.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Visual> CreateElementVisuals(XElement element, bool includeTextContent = false)
        {
            var childNodeList = element.Nodes().ToArray();
            for (int i = 0; i < childNodeList.Length; i++)
            {
                var childNode = childNodeList[i];

                // if we're ignoring text, skip text nodes
                if (!includeTextContent && childNode is XText)
                    continue;

                // restore existing visual child, if it exists
                var visual = nodeChildren.GetOrDefault(childNode);
                if (visual != null)
                    yield return visual;

                // create new child
                visual = Document.CreateVisual(this, childNode);
                if (visual != null)
                {
                    nodeChildren[childNode] = visual;
                    yield return visual;
                }
            }
        }

        /// <summary>
        /// Invalidates the children collection.
        /// </summary>
        protected void InvalidateChildren()
        {
            children = null;

            OnChildrenInvalidated(EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the children of this <see cref="Visual"/> have been invalidated.
        /// </summary>
        public event EventHandler<EventArgs> VisualsInvalidated;

        /// <summary>
        /// Raises the ChildrenInvalidated event.
        /// </summary>
        /// <param name="args"></param>
        void OnChildrenInvalidated(EventArgs args)
        {
            if (VisualsInvalidated != null)
                VisualsInvalidated(this, args);
        }

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Visual> Descendants()
        {
            return Descendants(false);
        }

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Visual> Descendants(bool includeSelf)
        {
            if (includeSelf)
                yield return this;

            foreach (var child in Visuals)
                if (child is ContentVisual)
                    foreach (var descendant in ((ContentVisual)child).Descendants(true))
                        yield return descendant;
                else
                    yield return child;
        }

        /// <summary>
        /// Yields each descendant visual, ignoring visuals and their children that do not satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <param name="includeSelf"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Visual> Descendants(bool includeSelf, Func<Visual, bool> predicate)
        {
            if (!predicate(this))
                yield break;

            if (includeSelf)
                yield return this;

            foreach (var child in Visuals)
                if (child is ContentVisual)
                    foreach (var descendant in ((ContentVisual)child).Descendants(true, predicate))
                        yield return descendant;
                else if (predicate(child))
                    yield return child;
        }

        /// <summary>
        /// Yields each descendant visual.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Visual> DescendantsIncludeNS(INamingScope[] ns)
        {
            if (!ns.Contains(GetNamingScope(this)))
                yield break;

            yield return this;

            foreach (var child in Visuals)
                if (child is ContentVisual)
                    foreach (var descendant in ((ContentVisual)child).DescendantsIncludeNS(ns))
                        yield return descendant;
                else
                    yield return child;
        }

        /// <summary>
        /// Gets the stored <see cref="Visual"/>-state of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetState<T>()
            where T : new()
        {
            return Document.VisualState.Get<T>(this);
        }

    }

}
