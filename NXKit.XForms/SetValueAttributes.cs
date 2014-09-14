using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'setvalue' attributes.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}setvalue")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SetValueAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public SetValueAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'value' attribute value.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

    }

}