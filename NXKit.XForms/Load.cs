using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}load")]
    public class Load : 
        IAction
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Load(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        XFormsModule Module
        {
            get { return element.Host().Module<XFormsModule>(); }
        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

    }

}
