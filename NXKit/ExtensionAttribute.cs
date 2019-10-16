using System;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Marks the interface object as being associated with a type of object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExtensionAttribute : ExportAttribute, IExtensionMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ExtensionAttribute() :
            this((Type)null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        public ExtensionAttribute(Type contractType) :
            this(contractType, ExtensionObjectType.Element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        public ExtensionAttribute(ExtensionObjectType objectType) :
            this(null, objectType)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="objectType"></param>
        public ExtensionAttribute(Type contractType, ExtensionObjectType objectType) :
            base(contractType, CompositionScope.Object)
        {
            ObjectType = objectType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ExtensionAttribute(XName name) :
            this((Type)null, name)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="name"></param>
        public ExtensionAttribute(Type contractType, XName name) :
            this(contractType, name.NamespaceName, name.LocalName)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public ExtensionAttribute(string expandedName) :
            this((Type)null, expandedName)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="expandedName"></param>
        public ExtensionAttribute(Type contractType, string expandedName) :
            this(contractType, XName.Get(expandedName))
        {
            if (expandedName == null)
                throw new ArgumentNullException(nameof(expandedName));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public ExtensionAttribute(string namespaceName, string localName) :
            this((Type)null, namespaceName, localName)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public ExtensionAttribute(Type contractType, string namespaceName, string localName) :
            this(contractType, ExtensionObjectType.Element)
        {
            NamespaceName = namespaceName;
            LocalName = localName;
        }

        /// <summary>
        /// Gets the type of node this interface applies to.
        /// </summary>
        public ExtensionObjectType ObjectType { get; }

        /// <summary>
        /// Gets the expanded name.
        /// </summary>
        XName Name => NamespaceName != null && LocalName != null ? XName.Get(LocalName, NamespaceName) : null;

        /// <summary>
        /// Gets the associated name.
        /// </summary>
        public string ExpandedName => Name?.ToString();

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        public string NamespaceName { get; set; }

        /// <summary>
        /// Gets the local name.
        /// </summary>
        public string LocalName { get; }

        /// <summary>
        /// Specifies the predicate type to determine whether this interface applies to the decorated <see cref="XObject"/>.
        /// </summary>
        public Type PredicateType { get; set; }

    }

}
