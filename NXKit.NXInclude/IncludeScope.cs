using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.NXInclude
{

    [Extension(typeof(IncludeScopePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeScope :
        ElementExtension,
        IRefScope
    {

        public class IncludeScopePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Annotation<IncludeScopeAnnotation>() != null;
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public IncludeScope(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
