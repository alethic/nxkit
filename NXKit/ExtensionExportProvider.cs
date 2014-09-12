using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit
{

    [Export(typeof(IExportProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ExtensionExportProvider :
        IExportProvider
    {

        readonly XObject obj;
        readonly IEnumerable<Lazy<IExtensionProvider>> providers;
        readonly Dictionary<Tuple<object, string>, Export> exports;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="providers"></param>
        [ImportingConstructor]
        public ExtensionExportProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtensionProvider>> providers)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(providers != null);

            this.obj = obj;
            this.providers = providers;
            this.exports = new Dictionary<Tuple<object, string>, Export>();
        }

        Export GetExport(object extension, string contractName)
        {
            return exports.GetOrAdd(Tuple.Create(extension, contractName), () => new Export(contractName, () => extension));
        }

        public IEnumerable<Export> GetExports(ExportProvider source, ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var typeName = Type.GetType(definition.ContractName);

            return providers
                .SelectMany(i => i.Value.GetExtensions(obj, typeName))
                .Select(i => new Export(definition.ContractName, () => i));
        }

    }

}
