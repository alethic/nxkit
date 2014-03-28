namespace NXKit.Web
{

    /// <summary>
    /// Hosts a <see cref="NXDocument"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class NXDocumentHost
    {

        readonly NXDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXDocumentHost()
        {

        }

        /// <summary>
        /// Gets the currently hosted <see cref="NXDocument"/>.
        /// </summary>
        public NXDocument Document
        {
            get { return document; }
        }

    }

}
