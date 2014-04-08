using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

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

    }

}
