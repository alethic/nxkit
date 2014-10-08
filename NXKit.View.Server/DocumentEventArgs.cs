using System;
using System.Diagnostics.Contracts;

namespace NXKit.View.Server
{

    public class DocumentEventArgs :
        EventArgs
    {

        readonly Document document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentEventArgs(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        /// <summary>
        /// Gets the effected <see cref="Document"/>.
        /// </summary>
        public Document Document
        {
            get { return document; }
        }

    }

}
