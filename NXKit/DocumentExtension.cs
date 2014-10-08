using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base class for an extension on <see cref="XDocument"/> instances.
    /// </summary>
    public abstract class DocumentExtension :
        NodeExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentExtension(XDocument document)
            : base(document)
        {
            Contract.Requires<ArgumentNullException>(document != null);
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> this extension applies to.
        /// </summary>
        public XDocument Document
        {
            get { return (XDocument)Node; }
        }

    }

}
