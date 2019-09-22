using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// 3.3.1 Common Attributes
    /// 
    /// The Common Attribute Collection applies to every element in the XForms namespace.
    /// </summary>
    [Extension(typeof(CommonAttributes))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class CommonAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public CommonAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'model' attribute value.
        /// </summary>
        public IdRef Model
        {
            get { return GetAttributeValue("model"); }
        }

        /// <summary>
        /// Gets the 'context' attribute value.
        /// </summary>
        public string Context
        {
            get { return GetAttributeValue("context"); }
        }

        /// <summary>
        /// Gets the 'appearance' attribute.
        /// </summary>
        public XName Appearance
        {
            get { return GetAttributePrefixedName("appearance"); }
        }

    }

}
