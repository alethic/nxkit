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

        Stream Get(Uri uri);

        Stream Put(Uri uri, Stream stream);

    }

    [ContractClassFor(typeof(IResolver))]
    abstract class IResolver_Contract :
        IResolver
    {

        public Stream Get(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            throw new NotImplementedException();
        }

        public Stream Put(Uri uri, Stream stream)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            throw new NotImplementedException();
        }

    }

}
