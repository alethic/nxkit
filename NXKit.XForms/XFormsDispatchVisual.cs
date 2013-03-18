using System;

namespace NXKit.XForms
{

    [Visual("dispatch")]
    public class XFormsDispatchVisual : XFormsVisual, IActionVisual
    {

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
