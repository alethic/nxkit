using System;
using System.Xml.Linq;

using NXKit.View.Windows.Extensions;

namespace NXKit.XForms.View.Windows.Extensions
{

    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}label")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}help")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}hint")]
    [Extension(typeof(IViewMetadata), "{http://www.w3.org/2002/xforms}alert")]
    public class MetadataElements :
        ElementExtension,
        IViewMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public MetadataElements(XElement element)
            : base(element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

        public bool IsMetadata
        {
            get { return true; }
        }

    }

}
