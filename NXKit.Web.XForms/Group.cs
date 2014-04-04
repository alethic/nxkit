using System;
using System.Diagnostics.Contracts;

namespace NXKit.Web.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}group")]
    public class Group :
        Template
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Group(NXElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public override string Name
        {
            get { return "NXKit.XForms.Group"; }
        }

    }

}
