namespace NXKit.Web
{

    /// <summary>
    /// Hosts a <see cref="NXDocumentWebService"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class NXDocumentWebService
    {

        readonly NXDocumentWebService document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXDocumentWebService()
        {

        }

        /// <summary>
        /// Gets the currently hosted <see cref="NXDocumentWebService"/>.
        /// </summary>
        public NXDocumentWebService Document
        {
            get { return document; }
        }

    }

}
