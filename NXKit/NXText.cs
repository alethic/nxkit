using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a simple text node.
    /// </summary>
    [Public]
    public class NXText :
        NXNode
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXText()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXText(string value)
            : base()
        {
            Contract.Requires<ArgumentNullException>(value != null);

            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NXText(XText xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            this.value = xml.Value;
        }

        public new XText Xml
        {
            get { return (XText)base.Xml; }
        }

        /// <summary>
        /// Gets or sets the value of this node.
        /// </summary>
        [Public]
        public string Value
        {
            get { return value; }
        }

    }

}
