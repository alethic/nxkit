using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'send' element.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}send")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SendAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public SendAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'if' attribute values.
        /// </summary>
        public IdRef Submission
        {
            get { return GetAttributeValue("submission"); }
        }

    }

}