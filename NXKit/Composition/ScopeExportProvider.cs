using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    /// <summary>
    /// <see cref="ExportProvider"/> implementation that filters a parent <see cref="ExportProvider"/> for exports
    /// either within the designated scope or outside of it.
    /// </summary>
    class ScopeExportProvider :
        ExportProvider
    {

        readonly ExportProvider parent;
        readonly Scope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeExportProvider(ExportProvider parent, Scope scope = Scope.Global)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
            this.scope = scope;
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            return parent.TryGetExports(new ScopeImportDefinition(definition, scope), atomicComposition);
        }

    }

}
