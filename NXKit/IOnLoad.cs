namespace NXKit
{

    /// <summary>
    /// Invoked by the processor each time a document is loaded.
    /// </summary>
    public interface IOnLoad : IExtension
    {

        /// <summary>
        /// Invoked when the document has been loaded.
        /// </summary>
        void Load();

    }

}
