using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms action attributes.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DispatchAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public DispatchAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'name' attribute values.
        /// </summary>
        public string Name
        {
            get { return GetAttributeValue("name"); }
        }

        /// <summary>
        /// Gets the 'targetid' attribute values.
        /// </summary>
        public IdRef TargetId
        {
            get { return GetAttributeValue("targetid"); }
        }

        /// <summary>
        /// Gets the 'delay' attribute values.
        /// </summary>
        public string Delay
        {
            get { return GetAttributeValue("delay"); }
        }

        /// <summary>
        /// Gets the 'bubbles' attribute values.
        /// </summary>
        public string Bubbles
        {
            get { return GetAttributeValue("bubbles"); }
        }

        /// <summary>
        /// Gets the 'cancelable' attribute values.
        /// </summary>
        public string Cancelable
        {
            get { return GetAttributeValue("cancelable"); }
        }

    }

}