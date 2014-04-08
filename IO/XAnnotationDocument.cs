using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.IO
{

    /// <summary>
    /// Wraps an existing <see cref="XDocument"/> modifying the body contents to include persisted annotation data.
    /// </summary>
    public class XAnnotationDocument :
        XDocument
    {

        readonly XDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public XAnnotationDocument(XDocument document)
            : base(document.Declaration)
        {
            this.document = document;
            this.Add(GetContents());
        }

        /// <summary>
        /// Gets the source document.
        /// </summary>
        public XDocument Source
        {
            get { return document; }
        }

        /// <summary>
        /// Gets the contents of the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<object> GetContents()
        {
            if (document.DocumentType != null)
                yield return document.DocumentType;

            foreach (var node in document.Nodes())
                if (node is XElement)
                    // replace original element with annotating element.
                    yield return new XAnnotationRootElement((XElement)node);
                else
                    yield return node;
        }

    }

}
