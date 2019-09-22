using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Describes the available composition elements.
    /// </summary>
    public class CompositionConfiguration
    {

        static object sync = new object();
        static CompositionConfiguration defaultCompositionConfiguration;

        /// <summary>
        /// Gets the default <see cref="CompositionConfiguration"/>.
        /// </summary>
        public static CompositionConfiguration Default
        {
            get { return GetDefault() ; }
        }

        /// <summary>
        /// Implements the getter for Default.
        /// </summary>
        /// <returns></returns>
        static CompositionConfiguration GetDefault()
        {
            if (defaultCompositionConfiguration == null)
                lock (sync)
                    if (defaultCompositionConfiguration == null)
                        return defaultCompositionConfiguration = new CompositionConfiguration(
                            CompositionUtil.DefaultGlobalCatalog,
                            CompositionUtil.DefaultHostCatalog,
                            CompositionUtil.DefaultObjectCatalog,
                            new CompositionContainer());

            return defaultCompositionConfiguration;
        }


        readonly ScopeCatalog globalCatalog;
        readonly ScopeCatalog hostCatalog;
        readonly ScopeCatalog objectCatalog;
        readonly ExportProvider exports;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="globalCatalog"></param>
        /// <param name="hostCatalog"></param>
        /// <param name="objectCatalog"></param>
        /// <param name="exports"></param>
        public CompositionConfiguration(
            ScopeCatalog globalCatalog,
            ScopeCatalog hostCatalog,
            ScopeCatalog objectCatalog,
            ExportProvider exports)
        {
            this.globalCatalog = globalCatalog ?? throw new ArgumentNullException(nameof(globalCatalog));
            this.hostCatalog = hostCatalog ?? throw new ArgumentNullException(nameof(hostCatalog));
            this.objectCatalog = objectCatalog ?? throw new ArgumentNullException(nameof(objectCatalog));
            this.exports = exports ?? throw new ArgumentNullException(nameof(exports));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        public CompositionConfiguration(
            ComposablePartCatalog catalog, 
            ExportProvider exports)
            : this(
                new ScopeCatalog(catalog, Scope.Global),
                new ScopeCatalog(catalog, Scope.Host),
                new ScopeCatalog(catalog, Scope.Object),
                exports)
        {
            if (catalog == null)
                throw new ArgumentNullException(nameof(catalog));
            if (exports == null)
                throw new ArgumentNullException(nameof(exports));
        }

        /// <summary>
        /// Gets the catalog to provide global parts.
        /// </summary>
        public ScopeCatalog GlobalCatalog
        {
            get { return globalCatalog; }
        }

        /// <summary>
        /// Gets hte catalog to provide host parts.
        /// </summary>
        public ScopeCatalog HostCatalog
        {
            get { return hostCatalog; }
        }

        /// <summary>
        /// Gets the catalog to provide object parts.
        /// </summary>
        public ScopeCatalog ObjectCatalog
        {
            get { return objectCatalog; }
        }

        /// <summary>
        /// Gets the external provider of exports.
        /// </summary>
        public ExportProvider Exports
        {
            get { return exports; }
        }

    }

}
