using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Composition
{

    /// <summary>
    /// Filters the available exports for a part down to those allowed within the requested scope.
    /// </summary>
    class ScopePartDefinition :
        ComposablePartDefinition
    {

        readonly ComposablePartDefinition parent;
        readonly Scope scope;
        readonly IEnumerable<ExportDefinition> exportDefinitions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public ScopePartDefinition(ComposablePartDefinition parent, Scope scope)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
            this.scope = scope;
            this.exportDefinitions = GetExportDefinitions().ToArray();
        }

        public override ComposablePart CreatePart()
        {
            return parent.CreatePart();
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return exportDefinitions; }
        }

        IEnumerable<ExportDefinition> GetExportDefinitions()
        {
            return parent.ExportDefinitions
                .Where(i => ScopeHelper.IsScoped(i.Metadata, scope));
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return parent.ImportDefinitions; }
        }

    }

}
