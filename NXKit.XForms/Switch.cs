using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}switch")]
    public class Switch
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Switch(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

    }

}
