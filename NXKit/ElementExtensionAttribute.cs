using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Exports an extension instance for a <see cref="XElement"/>.
    /// </summary>
    [MetadataAttribute]
    public class ElementExtensionAttribute :
        ObjectExtensionAttribute
    {

        readonly string namespaceName;
        readonly string localName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ElementExtensionAttribute()
            : base(ExtensionObjectType.Element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ElementExtensionAttribute(XName name)
            : this(name.NamespaceName, name.LocalName)
        {
            Contract.Requires<ArgumentNullException>(name != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="interfaceType"></param>
        public ElementExtensionAttribute(XName name, Type interfaceType)
            : this(name)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(interfaceType != null);

            InterfaceType = interfaceType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public ElementExtensionAttribute(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        /// <param name="interfaceType"></param>
        public ElementExtensionAttribute(string expandedName, Type interfaceType)
            : this(expandedName)
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
            Contract.Requires<ArgumentNullException>(interfaceType != null);

            InterfaceType = interfaceType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public ElementExtensionAttribute(string namespaceName, string localName)
            : this()
        {
            this.namespaceName = namespaceName;
            this.localName = localName;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        /// <param name="interfaceType"></param>
        public ElementExtensionAttribute(string namespaceName, string localName, Type interfaceType)
            : this(namespaceName, localName)
        {
            Contract.Requires<ArgumentNullException>(namespaceName != null);
            Contract.Requires<ArgumentNullException>(localName != null);
            Contract.Requires<ArgumentNullException>(interfaceType != null);

            InterfaceType = interfaceType;
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
        }

        /// <summary>
        /// Gets the local name.
        /// </summary>
        public string LocalName
        {
            get { return localName; }
        }

    }

}
