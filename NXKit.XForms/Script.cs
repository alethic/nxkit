using System;
using System.Diagnostics.Contracts;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [NXElement("{http://www.w3.org/2002/xforms}script")]
    [Public]
    public class Script :
        IAction
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Script(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

        public void Handle(Event evt)
        {
            Invoke();
        }

    }

}
