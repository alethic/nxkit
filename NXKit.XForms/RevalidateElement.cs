using System.Xml.Linq;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("revalidate")]
    public class RevalidateElement :
        XFormsElement,
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public RevalidateElement(XElement xml)
            : base(xml)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var modelAttr = Module.GetAttributeValue(Xml, "model");
            if (modelAttr != null)
            {
                var element = (NXElement)ResolveId(modelAttr);
                if (element != null)
                    element.Interface<Model>().OnRevalidate();
                else
                {
                    this.Interface<INXEventTarget>().DispatchEvent(Events.BindingException);
                    return;
                }
            }
        }

    }

}
