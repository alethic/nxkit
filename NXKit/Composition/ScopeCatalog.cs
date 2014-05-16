using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using NXKit.Util;

namespace NXKit.Composition
{

    /// <summary>
    /// Filters a parenet catalog for exports marked for the given <see cref="Scope"/>.
    /// </summary>
    public class ScopeCatalog :
        ComposablePartCatalog
    {

        /// <summary>
        /// Gets the part metadata key to describe the scope.
        /// </summary>
        public const string ScopeMetadataKey = "NXKit.Scope";

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
                .Where(i => GetScope(i.Metadata) == scope)
                .Buffer()
                .AsQueryable();
        }

        Scope GetScope(IDictionary<string, object> metadata)
        {
            return (Scope)metadata.GetOrValue(ScopeMetadataKey, Scope.Global);
        }

    }

}
