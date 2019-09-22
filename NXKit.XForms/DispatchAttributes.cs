using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms action attributes.
    /// </summary>
    [Extension(typeof(DispatchAttributes), "{http://www.w3.org/2002/xforms}dispatch")]
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
            if (element == null)
                throw new ArgumentNullException(nameof(element));
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