using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes an interface and the filters to allow it to apply to a <see cref="XObject"/>.
    /// </summary>
    public struct InterfaceDescriptor
    {

        readonly XmlNodeType nodeType;
        readonly string namespaceName;
        readonly string localName;
        readonly Type predicateType;
        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        /// <param name="predicateType"></param>
        /// <param name="type"></param>
        public InterfaceDescriptor(XmlNodeType nodeType, string namespaceName, string localName, Type predicateType, Type type)
        {
            this.nodeType = nodeType;
            this.namespaceName = namespaceName;
            this.localName = localName;
            this.predicateType = predicateType;
            this.type = type;
        }

        public XmlNodeType NodeType
        {
            get { return nodeType; }
        }

        public string NamespaceName
        {
            get { return namespaceName; }
        }

        public string LocalName
        {
            get { return localName; }
        }

        public Type PredicateType
        {
            get { return predicateType; }
        }

        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// Tests whether the given <see cref="XObject"/> matches the interface.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool IsMatch(XObject obj)
        {
            var self = this;

            if (nodeType != XmlNodeType.None &&
                nodeType != obj.NodeType)
                return false;

            // test against specified predicate type
            var predicate = predicateType != null ? (IInterfacePredicate)Activator.CreateInstance(predicateType) : null;
            if (predicate != null)
                if (!predicate.IsMatch(obj, type))
                    return false;

            var element = obj as XElement;
            if (element != null && !IsMatch(element))
                return false;

            return true;
        }

        /// <summary>
        /// Tests whether the given <see cref="XElement"/> matches the interface.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsMatch(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
                return false;

            return true;
        }

    }

}
