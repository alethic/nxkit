namespace NXKit.Server
{

    /// <summary>
    /// Provides the ability to resolve stored <see cref="Document"/> instances.
    /// </summary>
    public interface IDocumentStore
    {

        /// <summary>
        /// Gets a matching <see cref="Document"/> from the store.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Document Get(string hash);

        /// <summary>
        /// Sets the value into the store by it's hash key.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="document"></param>
        void Put(string hash, Document document);

    }

}
