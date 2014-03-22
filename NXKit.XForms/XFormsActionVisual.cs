using System.Linq;

using NXKit.DOM2.Events;

namespace NXKit.XForms
{

    [Visual("action")]
    public class XFormsActionVisual : 
        XFormsVisual,
        IActionVisual
    {

        public void Handle(IEvent ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            foreach (var action in Visuals.OfType<IActionVisual>())
                Module.InvokeAction(action);
        }

    }

}
