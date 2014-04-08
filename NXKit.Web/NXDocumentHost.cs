namespace NXKit.Web
{

    /// <summary>
    /// Hosts a <see cref="NXDocumentHost"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class NXDocumentHost
    {

        readonly NXDocumentHost document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXDocumentHost()
        {

        }

        /// <summary>
        /// Gets the currently hosted <see cref="NXDocumentHost"/>.
        /// </summary>
        public NXDocumentHost Document
        {
            get { return document; }
        }

    }

}
