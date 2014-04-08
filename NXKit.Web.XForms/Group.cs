using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Web.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}group")]
    public class Group :
        Template
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Group(XElement element)
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
