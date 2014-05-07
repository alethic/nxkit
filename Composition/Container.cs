using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    public abstract class Container :
        IContainer
    {

        readonly CompositionContainer container;
        readonly ComposablePartCatalog catalog;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="catalog"></param>
        public Container(CompositionContainer container, ComposablePartCatalog catalog)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(catalog != null);

            this.container = container;
            this.catalog = catalog;
        }

        public ComposablePartCatalog Catalog
        {
            get { return catalog; }
        }

        public CompositionContainer Exports
        {
            get { return container; }
        }

        public IEnumerable<Lazy<T, TMetadata>> GetExports<T, TMetadata>(Type contractType)
        {
            return container.GetExports<T, TMetadata>(AttributedModelServices.GetContractName(contractType));
        }

        public IEnumerable<Lazy<T, IDictionary<string, object>>> GetExports<T>(Type contractType)
        {
            return container.GetExports<T, IDictionary<string, object>>(AttributedModelServices.GetContractName(contractType));
        }

        public IEnumerable<Lazy<T>> GetExports<T>()
        {
            return container.GetExports<T>();
        }

        public IEnumerable<T> GetExportedValues<T>(Type contractType)
        {
            return container.GetExportedValues<T>(AttributedModelServices.GetContractName(contractType));
        }

        public IEnumerable<T> GetExportedValues<T>()
        {
            return container.GetExportedValues<T>();
        }

        public IEnumerable<object> GetExportedValues(Type contractType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(contractType));
        }

        public IEnumerable<Export> GetExports(ImportDefinition importDefinition)
        {
            return container.GetExports(importDefinition);
        }

        public IContainer WithExport<T>(T value)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(value != null);

            container.WithExport<T>(value);
            return this;
        }

        public IContainer WithExport(Type contractType, object value)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Requires<ArgumentNullException>(value != null);

            container.WithExport(contractType, value);
            return this;
        }

        public T GetExportedValue<T>()
        {
            return container.GetExportedValue<T>();
        }

        public T GetExportedValue<T>(Type contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);

            return container.GetExportedValue<T>(AttributedModelServices.GetContractName(contractType));
        }

    }

}
