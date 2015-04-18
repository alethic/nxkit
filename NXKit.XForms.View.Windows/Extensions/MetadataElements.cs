using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.View.Windows.Extensions;

namespace NXKit.XForms.View.Windows.Extensions
{

    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}label")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}help")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}hint")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}alert")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class MetadataElements :
        ElementExtension,
        IViewMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public MetadataElements(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public bool IsMetadata
        {
            get { return true; }
        }

    }

}
