using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.Imaging
{

    [Extension(typeof(RegionAttributes))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class RegionAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public RegionAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }


        /// <summary>
        /// Gets the value of the 'page' attribute.
        /// </summary>
        /// <returns></returns>
        public string Page
        {
            get { return GetAttributeValue("page"); }
        }

        /// <summary>
        /// Gets the value of the 'left' attribute.
        /// </summary>
        /// <returns></returns>
        public string Left
        {
            get { return GetAttributeValue("left"); }
        }

        /// <summary>
        /// Gets the value of the 'top' attribute.
        /// </summary>
        /// <returns></returns>
        public string Top
        {
            get { return GetAttributeValue("top"); }
        }

        /// <summary>
        /// Gets the value of the 'right' attribute.
        /// </summary>
        /// <returns></returns>
        public string Right
        {
            get { return GetAttributeValue("right"); }
        }

        /// <summary>
        /// Gets the value of the 'bottom' attribute.
        /// </summary>
        /// <returns></returns>
        public string Bottom
        {
            get { return GetAttributeValue("bottom"); }
        }

        /// <summary>
        /// Gets the value of the 'width' attribute.
        /// </summary>
        /// <returns></returns>
        public string Width
        {
            get { return GetAttributeValue("width"); }
        }

        /// <summary>
        /// Gets the value of the 'height' attribute.
        /// </summary>
        /// <returns></returns>
        public string Height
        {
            get { return GetAttributeValue("height"); }
        }

    }

}
