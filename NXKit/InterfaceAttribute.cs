using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Marks the interface object as being associated with a given fully qualified element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InterfaceAttribute :
        Attribute
    {

        readonly XmlNodeType nodeType;
        readonly string namespaceName;
        readonly string localName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InterfaceAttribute()
            :this(XmlNodeType.None)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InterfaceAttribute(XmlNodeType nodeType)
        {
            this.nodeType = nodeType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public InterfaceAttribute(XName name)
            : this(name.NamespaceName, name.LocalName)
        {
            Contract.Requires<ArgumentNullException>(name != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public InterfaceAttribute(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public InterfaceAttribute(string namespaceName, string localName)
            : this(XmlNodeType.Element)
        {
            this.namespaceName = namespaceName;
            this.localName = localName;
        }

        /// <summary>
        /// Gets the type of node this interface applies to.
        /// </summary>
        public XmlNodeType NodeType
        {
            get { return nodeType; }
        }

        /// <summary>
        /// Gets the expanded name.
        /// </summary>
        public XName Name
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
