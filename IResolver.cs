using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit
{

    /// <summary>
    /// Provides an interface for the forms processor to obtain and save resources.
    /// </summary>
    [ContractClass(typeof(IResolver_Contract))]
    public interface IResolver
    {

        Stream Get(Uri href);

        Stream Put(Uri href, Stream stream);

    }

    [ContractClassFor(typeof(IResolver))]
    class IResolver_Contract :
        IResolver
    {

        public Stream Get(Uri href)
        {
            Contract.Requires<ArgumentNullException>(href != null);
            throw new NotImplementedException();
        }

        public Stream Put(Uri href, Stream stream)
        {
            Contract.Requires<ArgumentNullException>(href != null);
            throw new NotImplementedException();
        }

    }

}
