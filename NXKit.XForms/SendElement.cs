using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("send")]
    public class SendElement :
        XFormsElement,
        IAction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SendElement(XElement xml)
            : base(xml)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var submissionAttr = Module.GetAttributeValue(this, "submission");
            if (submissionAttr != null)
            {
                var submissionVisual = ResolveId(submissionAttr);
                if (submissionVisual != null)
                    submissionVisual.Interface<INXEventTarget>().DispatchEvent(Events.Submit);
            }
        }

    }

}
