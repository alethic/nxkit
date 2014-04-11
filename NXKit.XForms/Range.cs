using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}range")]
    [Remote]
    public class Range :
        ElementExtension
    {

        readonly RangeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Range(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new RangeAttributes(Element);
        }

        [Remote]
        public int? Start
        {
            get { return attributes.Start; }
        }

        [Remote]
        public int? End
        {
            get { return attributes.End; }
        }

        [Remote]
        public int? Step
        {
            get { return attributes.Step; }
        }

    }

}
