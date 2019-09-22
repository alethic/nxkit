using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'submit' element.
    /// </summary>
    [Extension(typeof(SubmitAttributes), "{http://www.w3.org/2002/xforms}submit")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SubmitAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public SubmitAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Name != Constants.XForms_1_0 + "submit")
                throw new ArgumentException("", nameof(element));
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