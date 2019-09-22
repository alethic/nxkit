using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq.Expressions;

namespace NXKit.Composition
{

    /// <summary>
    /// Filters a parenet catalog for exports marked for the given <see cref="Scope"/>.
    /// </summary>
    public class ScopeCatalog :
        FilteredCatalog
    {

        /// <summary>
        /// Gets the part metadata key to describe the scope.
        /// </summary>
        public const string ScopeMetadataKey = "NXKit.Scope";

        /// <summary>
        /// Returns a function capable of filtering for parts within the given <see cref="Scope"/>.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        static Expression<Func<ComposablePartDefinition, bool>> GetFilter(Scope scope)
        {
            if (scope == Scope.Global)
                return d => d.ContainsPartMetadata(ScopeMetadataKey, Scope.Global) || !d.ContainsPartMetadataWithKey(ScopeMetadataKey);
            else if (scope == Scope.Host)
                return d => d.ContainsPartMetadata(ScopeMetadataKey, Scope.Host);
            else if (scope == Scope.Object)
                return d => d.ContainsPartMetadata(ScopeMetadataKey, Scope.Object);
            else
                return d => false;
        }

        readonly Scope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeCatalog(ComposablePartCatalog parent, Scope scope)
            : base(parent, GetFilter(scope))
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.scope = scope;
        }

    }

}
