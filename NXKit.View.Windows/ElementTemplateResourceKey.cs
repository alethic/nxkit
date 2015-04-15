using System;
using System.Diagnostics.Contracts;
using System.Windows.Markup;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    public class ElementTemplateResourceKey
    {

        XName name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ElementTemplateResourceKey()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ElementTemplateResourceKey(XName name)
            : this()
        {
            Contract.Requires<ArgumentNullException>(name != null);

            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public ElementTemplateResourceKey(string expandedName)
            : this(XName.Get(expandedName))
        {
            Contract.Requires<ArgumentNullException>(expandedName != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expandedName));
        }

        /// <summary>
        /// Gets or sets the <see cref="XName"/> referenced by this key.
        /// </summary>
        public XName Name
        {
            get { return name; }
            set { Contract.Requires<ArgumentNullException>(value != null); name = value; }
        }

        /// <summary>
        /// Gets or sets the expanded name referenced by this key.
        /// </summary>
        public string ExpandedName
        {
            get { return name.ToString(); }
            set { Contract.Requires<ArgumentNullException>(value != null); name = XName.Get(value); }
        }

    }

}
