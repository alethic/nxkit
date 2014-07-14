using System;
using System.Collections.Generic;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Implements <see cref="IExtensionMetadata"/>.
    /// </summary>
    public class ExtensionMetadata :
        IExtensionMetadata
    {

        /// <summary>
        /// Creates a <see cref="ExtensionMetadata"/> instance from the given metadata dictionary.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static IEnumerable<IExtensionMetadata> Extract(IDictionary<string, object> metadata)
        {
            var objectTypes = (ExtensionObjectType[])metadata.GetOrDefault("ObjectType");
            var localNames = (string[])metadata.GetOrDefault("LocalName");
            var namespaceNames = (string[])metadata.GetOrDefault("NamespaceName");
            var predicateTypes = (Type[])metadata.GetOrDefault("PredicateType");

            // find shortest array
            int length = objectTypes.Length;
            length = Math.Min(length, localNames.Length);
            length = Math.Min(length, namespaceNames.Length);
            length = Math.Min(length, predicateTypes.Length);

            // extract metadata
            for (int i = 0; i < length; i++)
                yield return new ExtensionMetadata(objectTypes[i], localNames[i], namespaceNames[i], predicateTypes[i]);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="localName"></param>
        /// <param name="namespaceName"></param>
        /// <param name="predicateType"></param>
        /// <param name="interfaceType"></param>
        public ExtensionMetadata(ExtensionObjectType objectType, string localName, string namespaceName, Type predicateType)
        {
            ObjectType = objectType;
            LocalName = localName;
            NamespaceName = namespaceName;
            PredicateType = predicateType;
        }

        public ExtensionObjectType ObjectType { get; set; }

        public string LocalName { get; set; }

        public string NamespaceName { get; set; }

        public Type PredicateType { get; set; }

    }

}
