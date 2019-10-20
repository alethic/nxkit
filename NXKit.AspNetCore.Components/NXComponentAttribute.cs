using System;
using System.Xml.Linq;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Marks the component object as being associated with a type of element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NXComponentAttribute : Attribute, INXComponentMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public NXComponentAttribute(XName name) :
            this(name.NamespaceName, name.LocalName)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public NXComponentAttribute(string expandedName) :
            this(XName.Get(expandedName))
        {
            if (expandedName == null)
                throw new ArgumentNullException(nameof(expandedName));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public NXComponentAttribute(string namespaceName, string localName)
        {
            NamespaceName = namespaceName;
            LocalName = localName;
        }

        /// <summary>
        /// Gets the expanded name.
        /// </summary>
        public XName Name => NamespaceName != null && LocalName != null ? XName.Get(LocalName, NamespaceName) : null;

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
