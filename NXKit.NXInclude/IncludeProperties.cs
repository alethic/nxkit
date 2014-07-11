using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.NXInclude
{

    [ElementExtension("{http://schemas.nxkit.org/2014/NXInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeProperties :
        XInclude.IncludeProperties
    {

        readonly IncludeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public IncludeProperties(XElement element)
            : base(element, element.AnnotationOrCreate<IncludeAttributes>(() => new IncludeAttributes(element)))
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
