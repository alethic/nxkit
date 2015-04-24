using System;
using System.Diagnostics.Contracts;

namespace NXKit.View.Server
{

    /// <summary>
    /// Provides the ability to resolve stored <see cref="Document"/> instances.
    /// </summary>
    [ContractClass(typeof(IDocumentStore_Contract))]
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

    [ContractClassFor(typeof(IDocumentStore))]
    abstract class IDocumentStore_Contract :
        IDocumentStore
    {

        public Document Get(string hash)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            throw new NotImplementedException();
        }

        public void Put(string hash, Document document)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentNullException>(document != null);
            throw new NotImplementedException();
        }

    }

}
