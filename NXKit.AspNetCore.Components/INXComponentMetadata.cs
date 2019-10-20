using System;

using NXKit.Composition;

namespace NXKit.AspNetCore.Components
{

    public interface INXComponentMetadata
    {

        /// <summary>
        /// Gets the object local name to filter.
        /// </summary>
        string LocalName { get; }

        /// <summary>
        /// Gets the object namespace name to filter.
        /// </summary>
        string NamespaceName { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of a predicate that will evaluate whether the component applies.
        /// </summary>
        Type PredicateType { get; }

    }

}