using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.NXInclude
{

    [Extension(typeof(IncludeAttributes))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeAttributes :
        XInclude.IncludeAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public IncludeAttributes(XElement element)
            : base(element, "http://schemas.nxkit.org/2014/NXInclude")
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
