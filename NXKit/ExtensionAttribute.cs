using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Marks the interface object as being associated with a given fully qualified element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [MetadataAttribute]
    public class ExtensionAttribute :
        ExportAttribute
    {

        ExtensionObjectType objectType;
        string namespaceName;
        string localName;
        Type predicateType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        public ExtensionAttribute(Type contractType)
            : this(contractType, ExtensionObjectType.Element)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ExtensionAttribute()
            : this(ExtensionObjectType.Element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="objectType"></param>
        public ExtensionAttribute(Type contractType, ExtensionObjectType objectType)
            : base(contractType)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);

            this.objectType = objectType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="objectType"></param>
        public ExtensionAttribute(ExtensionObjectType objectType)
            : base(typeof(IExtension))
        {
            this.objectType = objectType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="name"></param>
        public ExtensionAttribute(Type contractType, XName name)
            : this(contractType, name.NamespaceName, name.LocalName)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Requires<ArgumentNullException>(name != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ExtensionAttribute(XName name)
            : this(name.NamespaceName, name.LocalName)
        {
            Contract.Requires<ArgumentNullException>(name != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="expandedName"></param>
        public ExtensionAttribute(Type contractType, string expandedName)
            : this(contractType, XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(contractType != null);
            Contract.Requires<ArgumentNullException>(expandedName != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public ExtensionAttribute(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public ExtensionAttribute(Type contractType, string namespaceName, string localName)
            : this(contractType, ExtensionObjectType.Element)
        {
            Contract.Requires<ArgumentNullException>(contractType != null);

            this.namespaceName = namespaceName;
            this.localName = localName;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public ExtensionAttribute(string namespaceName, string localName)
            : this(ExtensionObjectType.Element)
        {
            this.namespaceName = namespaceName;
            this.localName = localName;
        }

        /// <summary>
        /// Gets the type of node this interface applies to.
        /// </summary>
        public ExtensionObjectType ObjectType
        {
            get { return objectType; }
        }

        /// <summary>
        /// Gets the expanded name.
        /// </summary>
        XName Name
        {
            get { return namespaceName != null && localName != null ? XName.Get(localName, namespaceName) : null; }
        }

        /// <summary>
        /// Gets the associated name.
        /// </summary>
        public string ExpandedName
        {
            get { return Name != null ? Name.ToString() : null; }
        }

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        public string NamespaceName
        {
            get { return namespaceName; }
            set { namespaceName = value; }
        }

        /// <summary>
        /// Gets the local name.
        /// </summary>
        public string LocalName
        {
            get { return localName; }
        }

        /// <summary>
        /// Specifies the predicate type to determine whether this interface applies to the decorated <see cref="XObject"/>.
        /// </summary>
        public Type PredicateType
        {
            get { return predicateType; }
            set { predicateType = value; }
        }

    }

}
