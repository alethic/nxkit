using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Marks the interface object as being associated with a given fully qualified element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NXElementInterfaceAttribute :
        Attribute
    {

        readonly string namespaceName;
        readonly string localName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXElementInterfaceAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public NXElementInterfaceAttribute(XName name)
            : this()
        {
            Contract.Requires<ArgumentNullException>(name != null);

            this.namespaceName = name.NamespaceName;
            this.localName = name.LocalName;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public NXElementInterfaceAttribute(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public NXElementInterfaceAttribute(string namespaceName, string localName)
            : this()
        {
            this.namespaceName = namespaceName;
            this.localName = localName;
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
