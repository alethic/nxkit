namespace NXKit
{

    /// <summary>
    /// Invoked by the processor before a document is saved.
    /// </summary>
    public interface IOnSave
    {

        /// <summary>
        /// Invoked when the document is being saved.
        /// </summary>
        void Save();

    }

}
