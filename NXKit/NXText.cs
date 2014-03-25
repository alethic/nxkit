using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a simple text node.
    /// </summary>
    public class NXText :
        NXNode
    {

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
        /// <param name="text"></param>
        public NXText(XText text)
            : base(text)
        {
            Contract.Requires<ArgumentNullException>(text != null);
        }

        public XText Xml
        {
            get { return (XText)base.Xml; }
        }

        [Interactive]
        public string Text
        {
            get { return Xml.Value; }
        }

    }

}
