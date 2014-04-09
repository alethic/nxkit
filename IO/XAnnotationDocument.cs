using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.IO
{

    /// <summary>
    /// Wraps an existing <see cref="XDocument"/> modifying the body contents to include persisted annotation data.
    /// </summary>
    public class XAnnotationDocument :
        XDocument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public XAnnotationDocument(XDocument document)
            : base(document.Declaration)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            var l = GetContents(document).ToList();

            Add(l);
        }

        /// <summary>
        /// Gets the contents of the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<object> GetContents(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            if (document.DocumentType != null)
                yield return document.DocumentType;

            foreach (var node in document.Nodes())
                if (node is XElement)
                    // replace original element with annotating element.
                    yield return new XAnnotationElement((XElement)node);
                else
                    yield return node;
        }

    }

}
