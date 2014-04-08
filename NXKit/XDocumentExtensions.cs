using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.IO;

namespace NXKit
{

    /// <summary>
    /// Provides extension methods for <see cref="XDocument"/> instances.
    /// </summary>
    public static class XDocumentExtensions
    {

        /// <summary>
        /// Returns a collection of descendant nodes for this document, including the document itself.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<XNode> DescendantsAndSelf(this XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            yield return document;
            foreach (var node in document.DescendantNodes())
                yield return node;
        }

        /// <summary>
        /// Creates a <see cref="XmlReader"/> that includes annotation data in special nodes.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XmlReader CreateAnnotationReader(this XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return new XDocumentAnnotationReader(document);
        }

    }

}
