using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}message")]
    public class Message :
        IAction
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Message(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            //// ensure values are up to date
            //Refresh();

            //if (!(Binding is Node))
            //    return;

            //// set node value
            //Module.RaiseMessage(this);
        }

    }

}
