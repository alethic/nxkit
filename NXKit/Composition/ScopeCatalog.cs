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
        ComposablePartCatalog
    {

        /// <summary>
        /// Gets the part metadata key to describe the scope.
        /// </summary>
        public const string ScopeMetadataKey = "NXKit.Scope";

        readonly Scope scope;
        readonly IEnumerable<ComposablePartDefinition> parts;
        readonly Lazy<Dictionary<string, IEnumerable<ComposablePartDefinition>>> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="scope"></param>
        public ScopeCatalog(ComposablePartCatalog parent, Scope scope)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.scope = scope;
            this.parts = parent.Parts.Where(i => Filter(i)).ToList();
            this.cache = new Lazy<Dictionary<string, IEnumerable<ComposablePartDefinition>>>(() => CreateCache());
        }

        /// <summary>
        /// Creates the cache dictionary from the available parts.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IEnumerable<ComposablePartDefinition>> CreateCache()
        {
            return this
                .Select(i => i.ExportDefinitions
                    .Select(j => new
                    {
                        Part = i,
                        ContractName = j.ContractName,
                    }))
                .SelectMany(i => i)
                .GroupBy(i => i.ContractName)
                .ToDictionary(i => i.Key, i => (IEnumerable<ComposablePartDefinition>)i.Select(j => j.Part).ToList());
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

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return Enumerable.Empty<ComposablePartDefinition>()
                .Concat(GetFromCache(definition.ContractName))
                .Concat(GetFromCache((string)definition.Metadata.GetOrDefault(CompositionConstants.GenericContractMetadataName)))
                .SelectMany(i => i.ExportDefinitions
                    .Where(j => definition.IsConstraintSatisfiedBy(j))
                    .Select(j => Tuple.Create(i, j)));
        }

        /// <summary>
        /// Gets the parts which export the given contract name.
        /// </summary>
        /// <param name="contractName"></param>
        /// <returns></returns>
        IEnumerable<ComposablePartDefinition> GetFromCache(string contractName)
        {
            if (contractName == null)
                return Enumerable.Empty<ComposablePartDefinition>();
            else
                return cache.Value.GetOrValue(contractName, Enumerable.Empty<ComposablePartDefinition>());
        }

    }

}
