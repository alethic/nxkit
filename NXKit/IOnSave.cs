namespace NXKit
{

    /// <summary>
    /// Invoked by the processor before a document is saved.
    /// </summary>
    public interface IOnSave : IExtension
    {

        /// <summary>
        /// Invoked when the document is being saved.
        /// </summary>
        void Save();

    }

}
