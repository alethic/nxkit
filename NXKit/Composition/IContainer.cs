using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    [ContractClass(typeof(IContainer_Contract))]
    public interface IContainer
    {

        IEnumerable<Lazy<T, TMetadata>> GetExports<T, TMetadata>(Type contractType);

        IEnumerable<Lazy<T, IDictionary<string, object>>> GetExports<T>(Type contractType);

        IEnumerable<Lazy<T>> GetExports<T>();

        IEnumerable<T> GetExportedValues<T>(Type contractType);

        IEnumerable<T> GetExportedValues<T>();

        IEnumerable<object> GetExportedValues(Type contractType);

        IEnumerable<Export> GetExports(ImportDefinition importDefinition);

        IContainer WithExport<T>(T value) 
            where T : class;

        T GetExportedValue<T>();

        T GetExportedValue<T>(Type contractType);

    }

    [ContractClassFor(typeof(IContainer))]
    abstract class IContainer_Contract :
        IContainer
    {

        public IEnumerable<Lazy<T, TMetadata>> GetExports<T, TMetadata>(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Ensures(Contract.Result<IEnumerable<Lazy<T, TMetadata>>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<Lazy<T, IDictionary<string, object>>> GetExports<T>(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Ensures(Contract.Result<IEnumerable<Lazy<T, IDictionary<string, object>>>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<Lazy<T>> GetExports<T>()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Lazy<T>>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetExportedValues<T>(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetExportedValues<T>()
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetExportedValues(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);
            throw new NotImplementedException();
        }

        public IEnumerable<Export> GetExports(ImportDefinition importDefinition)
        {
            Contract.Requires<ArgumentNullException>(importDefinition != null);
            Contract.Ensures(Contract.Result<IEnumerable<Export>>() != null);
            throw new NotImplementedException();
        }

        public IContainer WithExport<T>(T value) where T : class
        {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Ensures(Contract.Result<IContainer>() != null);
            throw new NotImplementedException();
        }

        public T GetExportedValue<T>()
        {
            throw new NotImplementedException();
        }

        public T GetExportedValue<T>(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            throw new NotImplementedException();
        }

    }

}
