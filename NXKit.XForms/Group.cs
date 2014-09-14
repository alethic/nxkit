using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}group")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Group :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public Group(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
