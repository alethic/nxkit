using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("action")]
    public class XFormsActionVisual : 
        XFormsVisual,
        IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsActionVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            foreach (var action in Elements.OfType<IActionVisual>())
                Module.InvokeAction(action);
        }

    }

}
