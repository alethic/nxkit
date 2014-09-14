using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'delete' attributes.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DeleteAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public DeleteAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'at' attribute value.
        /// </summary>
        public string At
        {
            get { return GetAttributeValue("at"); }
        }

    }

}