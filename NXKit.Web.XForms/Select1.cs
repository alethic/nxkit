using System;
using System.Diagnostics.Contracts;

namespace NXKit.Web.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}select1")]
    public class Select1 :
        Template
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select1(NXElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public override string Name
        {
            get { return "NXKit.XForms.Select1"; }
        }

    }

}
