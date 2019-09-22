using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the select1 element.
    /// </summary>
    [Extension(typeof(Select1Attributes), "{http://www.w3.org/2002/xforms}select1")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Select1Attributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Select1Attributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        public string Incremetal
        {
            get { return GetAttributeValue("incremental"); }
        }

        public string Selection
        {
            get { return GetAttributeValue("selection"); }
        }

    }

}
