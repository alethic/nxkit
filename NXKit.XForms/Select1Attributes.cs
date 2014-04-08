using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the select1 element.
    /// </summary>
    public class Select1Attributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select1Attributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string Incremetal
        {
            get { return GetAttributeValue("incremental"); }
        }

        public string Selection
        {
            get { return GetAttributeValue("selection"); }
        }

    }

}
