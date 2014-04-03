using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Marks the interface object as being associated with a given fully qualified element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NXElementAttribute :
        Attribute
    {

        readonly string expandedName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public NXElementAttribute(XName name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            this.expandedName = name.ToString();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        tial public NXElementAttribute(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);

            this.expandedName = expandedName;
        }

        /// <summary>
        /// Gets the associated name.
        /// </summary>
        public string ExpandedName
        {
            get { return expandedName; }
        }

        /// <summary>
        /// Gets the expanded name.
        /// </summary>
        public XName Name
        {
            get { return XName.Get(expandedName); }
        }

    }

}
