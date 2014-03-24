using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("message")]
    public class XFormsMessageVisual : 
        XFormsSingleNodeBindingVisual, 
        IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsMessageVisual(XElement element)
            : base(element)
        {

        }

        public void Handle(Event ev)
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
