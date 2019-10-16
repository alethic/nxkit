namespace NXKit
{

    /// <summary>
    /// Invoked by the processor the first time a document is loaded.
    /// </summary>
    public interface IOnInit : IExtension
    {

        /// <summary>
        /// Invoked when the document has been initialized.
        /// </summary>
        void Init();

    }

}
