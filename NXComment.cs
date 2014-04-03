using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    public class NXComment :
        NXNode
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXComment()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NXComment(XComment xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);

            this.value = xml.Value;
        }

        /// <summary>
        /// Gets or sets the string value of this comment.
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }

}
