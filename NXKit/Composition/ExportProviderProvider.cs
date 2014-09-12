using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace NXKit.Composition
{

    /// <summary>
    /// Provides additional exports by querying the container itself for export providers.
    /// </summary>
    public class ExportProviderProvider :
        ExportProvider
    {

        ExportProvider source;
        IEnumerable<IExportProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ExportProviderProvider()
        {

        }

        IEnumerable<IExportProvider> Providers
        {
            get { return providers ?? (providers = GetProviders()); }
        }

        IEnumerable<IExportProvider> GetProviders()
        {
            return source
                .GetExports<IExportProvider>()
                .Select(i => i.Value)
                .ToList();
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            if (definition.ContractName == AttributedModelServices.GetContractName(typeof(IExportProvider)))
                return Enumerable.Empty<Export>();
            else
                return Providers.SelectMany(i => i.GetExports(source, definition, atomicComposition));
        }

        /// <summary>
        /// Export provider through which to query for <see cref="IExportProvider"/>s
        /// </summary>
        public ExportProvider Source
        {
            get { return source; }
            set { source = value; }
        }

    }

}
