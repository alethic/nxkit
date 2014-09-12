using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;


namespace NXKit.XForms
{

    /// <summary>
    /// Encapsulates the binding information for a UI element.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class UINode :
        ElementExtension,
        IUINode
    {

        readonly CommonAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public UINode(XElement element)
            :base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new CommonAttributes(Element);
        }

        /// <summary>
        /// Gets the next available <see cref="UIBinding"/> in the hierarchy.
        /// </summary>
        /// <returns></returns>
        UIBinding NextUIBinding()
        {
            return Element.AncestorsAndSelf()
                .SelectMany(i => i.Interfaces<IUIBindingNode>())
                .Select(i => i.UIBinding)
                .FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Gets whether or not the element is relevant.
        /// </summary>
        public bool Relevant
        {
            get { return GetRelevant(); }
        }

        /// <summary>
        /// Implements the getter for Relevant.
        /// </summary>
        /// <returns></returns>
        bool GetRelevant()
        {
            var next = NextUIBinding();
            if (next != null)
                return next.Relevant;

            return true;
        }

        /// <summary>
        /// Gets whether or not the element is considered read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return GetReadOnly(); }
        }

        /// <summary>
        /// Implements the getter for ReadOnly.
        /// </summary>
        /// <returns></returns>
        bool GetReadOnly()
        {
            var next = NextUIBinding();
            if (next != null)
                return next.ReadOnly;

            return true;
        }

        /// <summary>
        /// Gets whether or not the element is considered required.
        /// </summary>
        public bool Required
        {
            get { return GetRequired(); }
        }

        /// <summary>
        /// Implements the getter for Required.
        /// </summary>
        /// <returns></returns>
        bool GetRequired()
        {
            var next = NextUIBinding();
            if (next != null)
                return next.Required;

            return true;
        }

        /// <summary>
        /// Gets whether or not the element is considered valid.
        /// </summary>
        public bool Valid
        {
            get { return GetValid(); }
        }

        /// <summary>
        /// Implements the getter for Valid.
        /// </summary>
        /// <returns></returns>
        bool GetValid()
        {
            var next = NextUIBinding();
            if (next != null)
                return next.Valid;

            return true;
        }

        /// <summary>
        /// Author-optional attribute to define an appearance hint. If absent, the user agent may freely choose any suitable rendering.
        /// </summary>
        public XName Appearance
        {
            get { return attributes.Appearance; }
        }

    }

}
