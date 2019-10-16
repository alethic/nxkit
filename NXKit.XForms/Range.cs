using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}range")]
    [Remote]
    public class Range :
        ElementExtension
    {

        readonly RangeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public Range(
            XElement element,
            RangeAttributes attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
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
