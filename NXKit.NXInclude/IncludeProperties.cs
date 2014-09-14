using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.NXInclude
{

    [Extension("{http://schemas.nxkit.org/2014/NXInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeProperties :
        XInclude.IncludeProperties
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public IncludeProperties(
            XElement element,
            Extension<IncludeAttributes> attributes)
            : base(element, () => attributes.Value)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
        }

    }

}
