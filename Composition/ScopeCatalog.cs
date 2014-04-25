using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Composition
{

    class ScopeCatalog :
        ComposablePartCatalog
    {

        readonly ComposablePartCatalog parent;
        readonly Scope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeCatalog(ComposablePartCatalog parent, Scope scope)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.parent = parent;
            this.scope = scope;
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return GetParts(); }
        }

        IQueryable<ComposablePartDefinition> GetParts()
        {
            return parent.Parts
                .Select(i => new ScopePartDefinition(i, scope))
                .Where(i => i.ExportDefinitions.Any());
        }

    }

}
