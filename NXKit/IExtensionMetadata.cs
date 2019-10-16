using System;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Describes an extension's metadata.
    /// </summary>
    public interface IExtensionMetadata : IExportMetadata
    {

        /// <summary>
        /// Gets the type of object the extension applies to.
        /// </summary>
        ExtensionObjectType ObjectType { get; }

        /// <summary>
        /// Gets the object local name to filter.
        /// </summary>
        string LocalName { get; }

        /// <summary>
        /// Gets the object namespace name to filter.
        /// </summary>
        string NamespaceName { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of a predicate that will evaluate whether the extension applies.
        /// </summary>
        Type PredicateType { get; }

    }

}
