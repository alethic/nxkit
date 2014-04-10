using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}input")]
    public class Input :
        UIBindingNode
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Input(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
