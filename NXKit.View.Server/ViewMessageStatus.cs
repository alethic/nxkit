namespace NXKit.View.Server
{

    public enum ViewMessageStatus
    {

        Unknown = 0,

        /// <summary>
        /// Message succeeded.
        /// </summary>
        Good = 200,

        /// <summary>
        /// Document not found.
        /// </summary>
        NotFound = 400,

        /// <summary>
        /// An error occurred.
        /// </summary>
        Error = 500,

    }

}
