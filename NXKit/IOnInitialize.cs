namespace NXKit
{

    /// <summary>
    /// Invoked by the processor the first time a document is loaded.
    /// </summary>
    public interface IOnInitialize
    {

        /// <summary>
        /// Invoked when the document has been initialized.
        /// </summary>
        void Initialize();

    }

}
