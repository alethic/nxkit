using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'insert' element.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}insert")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class InsertAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public InsertAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "insert");
        }

        /// <summary>
        /// Gets the 'origin' attribute value.
        /// </summary>
        public string Origin
        {
            get { return GetAttributeValue("origin"); }
        }

        /// <summary>
        /// Gets the 'at' attribute value.
        /// </summary>
        public string At
        {
            get { return GetAttributeValue("at"); }
        }

        /// <summary>
        /// Gets the 'position' attribute value.
        /// </summary>
        public string Postition
        {
            get { return GetAttributeValue("position"); }
        }

    }

}