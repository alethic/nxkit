using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

using NXKit.Util;

namespace NXKit.Composition
{

    /// <summary>
    /// <see cref="ExportProvider"/> implementation that filters a parent <see cref="ExportProvider"/> for exports
    /// either within the designated scope or outside of it.
    /// </summary>
    class ScopeExportProvider :
        FilteredExportProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeExportProvider(ExportProvider parent, Scope scope = Scope.Global)
            : base(parent, _ => ((Scope?)_.Metadata.GetOrDefault<string, object>("Scope") ?? Scope.Global) == scope)
        {
            Contract.Requires<ArgumentNullException>(parent != null);
        }

    }

}
