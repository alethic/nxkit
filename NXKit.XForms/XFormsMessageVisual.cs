using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("message")]
    public class XFormsMessageVisual : 
        XFormsSingleNodeBindingVisual, 
        IActionVisual
    {

        public void Handle(IEvent ev)
        {
            Module.InvokeAction(this);
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
