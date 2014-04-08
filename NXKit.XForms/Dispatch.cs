using System;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}dispatch")]
    public class Dispatch :
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Dispatch(XElement element)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

    }

}
