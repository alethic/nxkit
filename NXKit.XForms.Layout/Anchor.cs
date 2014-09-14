using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}a")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Anchor :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Anchor(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
