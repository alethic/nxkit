using System;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("dispatch")]
    public class XFormsDispatchVisual : 
        XFormsVisual, 
        IActionVisual
    {

        public void Handle(IEvent ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

    }

}
