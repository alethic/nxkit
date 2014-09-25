namespace NXKit.Server
{

    /// <summary>
    /// Provides the ability to resolve cached <see cref="Document"/> state.
    /// </summary>
    public interface IDocumentCache
    {

        /// <summary>
        /// Gets a matching value from the cache if available.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        string Get(string hash);

        /// <summary>
        /// Sets the value into the cache by it's key.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="save"></param>
        void Set(string hash, string save);

    }

}
