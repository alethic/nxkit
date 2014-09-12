using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
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
        static Func<ComposablePartDefinition, bool> GetFilter(Scope scope)
        {
            return i => Filter(i, scope);
        }

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="ComposablePartDefinition"/> describes a part for the given
        /// <see cref="Scope"/>.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        static bool Filter(ComposablePartDefinition definition, Scope scope)
        {
            if (definition.ContainsPartMetadata(ScopeMetadataKey, scope))
                return true;
            else if (scope == Scope.Global && !definition.ContainsPartMetadataWithKey(ScopeMetadataKey))
                return true;
            else
                return false;
        }

        readonly Scope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeCatalog(ComposablePartCatalog parent, Scope scope)
            :base(parent, GetFilter(scope))
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.scope = scope;
        }

    }

}
