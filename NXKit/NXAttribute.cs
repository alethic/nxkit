using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    public class NXAttribute :
        NXObject
    {

        static IEnumerable<NXAttribute> emptySequence = new NXAttribute[0];

        /// <summary>
        /// Gets an empty collection of attributes.
        /// </summary>
        public static IEnumerable<NXAttribute> EmptySequence
        {
            get { return emptySequence; }
        }

        /// <summary>
        /// Returns the string value of the attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static explicit operator string(NXAttribute attribute)
        {
            return attribute == null ? null : attribute.value;
        }


        readonly XName name;
        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public NXAttribute(XName name, string value)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(value != null);

            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NXAttribute(XAttribute xml)
            : this(xml.Name, xml.Value)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
        }

        /// <summary>
        /// Determines if this attribute is a namespace declaration.
        /// </summary>
        public bool IsNamespaceDeclaration
        {
            get { return name.NamespaceName.Length != 0 ? name.NamespaceName == "http://www.w3.org/2000/xmlns/" : name.LocalName == "xmlns"; }
        }

        /// <summary>
        /// Gets the expanded name of this attribute.
        /// </summary>
        public XName Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the value of this attribute.
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        internal string GetPrefixOfNamespace(XNamespace ns)
        {
            if (ns.NamespaceName.Length == 0)
                return string.Empty;
            
            //if (Parent != null)
            //    return ((NXElement)Parent).GetPrefixOfNamespace(ns);

            if (ns.NamespaceName == "http://www.w3.org/XML/1998/namespace")
                return "xml";

            if (ns.NamespaceName == "http://www.w3.org/2000/xmlns/")
                return "xmlns";

            return null;
        }

        public void Remove()
        {
            Contract.Requires<InvalidOperationException>(Parent != null);

            ((NXElement)Parent).RemoveAttribute(this);
        }

        public void SetValue(object value)
        {
            Contract.Requires<ArgumentNullException>(value != null);
            
            Value = value.ToString();
        }

    }

}
