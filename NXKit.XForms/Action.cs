using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("action")]
    public class Action :
        XFormsElement,
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Action(XElement xml)
            : base(xml)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            foreach (var action in Elements().OfType<IAction>())
                Module.InvokeAction(action);
        }

    }

}
