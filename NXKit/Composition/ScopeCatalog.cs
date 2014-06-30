using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

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

            this.scope = scope;
            this.parts = parent.Parts.Where(i => Filter(i)).ToList().AsQueryable();
        }

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="ComposablePartDefinition"/> describes a part for the given
        /// <see cref="Scope"/>.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        bool Filter(ComposablePartDefinition definition)
        {
            if (definition.ContainsPartMetadata(ScopeMetadataKey, scope))
                return true;
            else if (scope == Scope.Global && !definition.ContainsPartMetadataWithKey(ScopeMetadataKey))
                return true;
            else
                return false;
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return parts; }
        }

    }

}
