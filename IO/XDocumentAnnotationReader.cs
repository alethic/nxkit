using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.IO
{

    /// <summary>
    /// Provides a reader for a <see cref="XDocument"/> instance that attempts to provided serialized representations
    /// of annotation data.
    /// </summary>
    public class XDocumentAnnotationReader :
        XNodeReaderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public XDocumentAnnotationReader(XDocument document)
            : base(new XAnnotationDocument(document))
        {
            Contract.Requires<ArgumentNullException>(document != null);
        }

        public new XDocument Source
        {
            get { return (XDocument)base.Source; }
        }

    }

}
