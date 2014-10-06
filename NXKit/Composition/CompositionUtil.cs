using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    /// <summary>
    /// Composition utilities.
    /// </summary>
    public static class CompositionUtil
    {

        readonly static object sync = new object();
        static ComposablePartCatalog defaultCatalog;
        static ScopeCatalog defaultGlobalCatalog;
        static ScopeCatalog defaultHostCatalog;
        static ScopeCatalog defaultObjectCatalog;

        /// <summary>
        /// Gets the default catalog.
        /// </summary>
        public static ComposablePartCatalog DefaultCatalog
        {
            get { return GetDefaultCatalog(); }
        }

        /// <summary>
        /// Implements the getter for DefaultCatalog
        /// </summary>
        /// <returns></returns>
        static ComposablePartCatalog GetDefaultCatalog()
        {
            if (defaultCatalog == null)
                lock (sync)
                    if (defaultCatalog == null)
                        defaultCatalog = new ApplicationCatalog();

            return defaultCatalog;
        }

        public static ScopeCatalog DefaultGlobalCatalog
        {
            get { return GetDefaultGlobalCatalog(); }
        }

        /// <summary>
        /// Implements the getter for DefaultGlobalCatalog
        /// </summary>
        /// <returns></returns>
        static ScopeCatalog GetDefaultGlobalCatalog()
        {
            if (defaultGlobalCatalog == null)
                lock (sync)
                    if (defaultGlobalCatalog == null)
                        defaultGlobalCatalog = new ScopeCatalog(DefaultCatalog, Scope.Global);

            return defaultGlobalCatalog;
        }

        public static ScopeCatalog DefaultHostCatalog
        {
            get { return GetDefaultHostCatalog(); }
        }

        /// <summary>
        /// Implements the getter for DefaultHostCatalog
        /// </summary>
        /// <returns></returns>
        static ScopeCatalog GetDefaultHostCatalog()
        {
            if (defaultHostCatalog == null)
                lock (sync)
                    if (defaultHostCatalog == null)
                        defaultHostCatalog = new ScopeCatalog(DefaultCatalog, Scope.Host);

            return defaultHostCatalog;
        }

        public static ScopeCatalog DefaultObjectCatalog
        {
            get { return GetDefaultObjectCatalog(); }
        }

        /// <summary>
        /// Implements the getter for DefaultObjectCatalog
        /// </summary>
        /// <returns></returns>
        static ScopeCatalog GetDefaultObjectCatalog()
        {
            if (defaultObjectCatalog == null)
                lock (sync)
                    if (defaultObjectCatalog == null)
                        defaultObjectCatalog = new ScopeCatalog(DefaultCatalog, Scope.Object);

            return defaultObjectCatalog;
        }

        /// <summary>
        /// Creates a new <see cref="CompositionContainer"/>.
        /// </summary>
        /// <returns></returns>
        public static CompositionContainer CreateContainer(ComposablePartCatalog catalog)
        {
            Contract.Requires<ArgumentNullException>(catalog != null);
            
            return ConfigureContainer(new CompositionContainer(catalog));
        }

        /// <summary>
        /// Injects a reference to the container into the container.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static CompositionContainer ConfigureContainer(CompositionContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            if (container.GetExportedValueOrDefault<ContainerRef>() == null)
                container.WithExport<ContainerRef>(new ContainerRef(container));

            return container;
        }

        /// <summary>
        /// Creates a new <see cref="CompositionContainer"/> using the default catalog.
        /// </summary>
        /// <returns></returns>
        public static CompositionContainer CreateContainer()
        {
            return CreateContainer(defaultCatalog);
        }

        /// <summary>
        /// Adds the specified value to the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="value"></param>
        public static CompositionContainer WithExport<T>(this CompositionContainer container, T value)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(value != null);

            container.ComposeExportedValue(value);

            return container;
        }

        /// <summary>
        /// Adds the specified value to the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="contractType"></param>
        /// <param name="value"></param>
        public static CompositionContainer WithExport(this CompositionContainer container, Type contractType, object value)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Requires<ArgumentNullException>(value != null);

            container.ComposeExportedValue(AttributedModelServices.GetContractName(contractType), value);

            return container;
        }

    }

}
