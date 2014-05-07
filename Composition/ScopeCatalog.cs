using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit.Composition
{

    /// <summary>
    /// Filters a parenet catalog for exports marked for the given <see cref="Scope"/>.
    /// </summary>
    class ScopeCatalog :
        ComposablePartCatalog
    {

        readonly ComposablePartCatalog parent;
        readonly Scope scope;
        readonly IQueryable<ComposablePartDefinition> parts;

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
            this.parts = GetParts();
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return parts; }
        }

        IQueryable<ComposablePartDefinition> GetParts()
        {
            return parent.Parts
                .Select(i => new ScopePartDefinition(i, scope))
                .Where(i => i.ExportDefinitions.Any())
                .Buffer()
                .AsQueryable();
        }

    }

}
