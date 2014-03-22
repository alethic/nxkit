using NXKit.DOM2.Events;

namespace NXKit.XForms
{

    [Visual("load")]
    public class XFormsLoadVisual : 
        XFormsSingleNodeBindingVisual,
        IActionVisual
    {

        public void Handle(IEvent ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();


        }

    }

}
