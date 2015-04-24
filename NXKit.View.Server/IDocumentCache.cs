using System;
using System.Diagnostics.Contracts;

namespace NXKit.View.Server
{

    /// <summary>
    /// Provides the ability to resolve cached <see cref="Document"/> state.
    /// </summary>
    [ContractClass(typeof(IDocumentCache_Contract))]
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

    [ContractClassFor(typeof(IDocumentCache))]
    abstract class IDocumentCache_Contract :
        IDocumentCache
    {

        public string Get(string hash)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            throw new NotImplementedException();
        }

        public void Set(string hash, string save)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentNullException>(save != null);
            throw new NotImplementedException();
        }

    }

}
