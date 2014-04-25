using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using NXKit.Util;

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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        public ScopePartDefinition(ComposablePartDefinition parent, Scope scope)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
            this.scope = scope;
        }

        public override ComposablePart CreatePart()
        {
            return parent.CreatePart();
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return GetExportDefinitions(); }
        }

        IEnumerable<ExportDefinition> GetExportDefinitions()
        {
            return parent.ExportDefinitions
                .Where(i => GetScopeFromMetadata(i.Metadata) == scope);
        }

        Scope GetScopeFromMetadata(IDictionary<string, object> metadata)
        {
            var data = metadata.GetOrDefault("Scope");
            if (data == null)
                return Scope.Global;

            if (data is Scope)
                return (Scope)data;

            var array = data as IEnumerable;
            if (array != null)
            {
                var scope = ((IEnumerable)data).Cast<object>().FirstOrDefault();
                if (scope != null)
                    return (Scope)scope;
            }

            return Scope.Global;
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return parent.ImportDefinitions; }
        }

    }

}
