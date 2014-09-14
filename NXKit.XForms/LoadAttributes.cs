using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'load' element attributes.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}load")]
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "load");
        }

        /// <summary>
        /// Gets the 'resource' attribute value.
        /// </summary>
        public string Resource
        {
            get { return GetAttributeValue("resource"); }
        }

        /// <summary>
        /// Gets the 'show' attribute value.
        /// </summary>
        public string Show
        {
            get { return GetAttributeValue("show"); }
        }

        /// <summary>
        /// Gets the 'targetid' attribute value.
        /// </summary>
        public string TargetId
        {
            get { return GetAttributeValue("targetid"); }
        }

    }

}