using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("action")]
    public class XFormsActionVisual : XFormsVisual, IActionVisual
    {

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            foreach (var action in Children.OfType<IActionVisual>())
                Module.InvokeAction(action);
        }

    }

}
