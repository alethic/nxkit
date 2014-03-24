using System.Xml.Linq;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("load")]
    public class XFormsLoadVisual : 
        XFormsSingleNodeBindingVisual,
        IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsLoadVisual(XElement element)
            : base(element)
        {

        }

        public void Handle(Event ev)
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
