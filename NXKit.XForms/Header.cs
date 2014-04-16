using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    public class Header :
        ElementExtension
    {

        readonly HeaderAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Header(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentException>(element.Name == Constants.XForms_1_0 + "header");

            this.attributes = new HeaderAttributes(element);
        }

        internal IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders()
        {
            yield break;
        }

    }

}
