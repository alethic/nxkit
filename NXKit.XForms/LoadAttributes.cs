using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'load' element attributes.
    /// </summary>
    [Extension(typeof(LoadAttributes), "{http://www.w3.org/2002/xforms}load")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class LoadAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public LoadAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Name != Constants.XForms_1_0 + "load")
                throw new ArgumentException("", nameof(element));
        }

        /// <summary>
        /// Gets the 'resource' attribute value.
        /// </summary>
        public string Resource => GetAttributeValue("resource");

        /// <summary>
        /// Gets the 'show' attribute value.
        /// </summary>
        public string Show => GetAttributeValue("show");

        /// <summary>
        /// Gets the 'targetid' attribute value.
        /// </summary>
        public string TargetId => GetAttributeValue("targetid");

    }

}