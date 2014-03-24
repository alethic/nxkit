using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("send")]
    public class XFormsSendVisual : 
        XFormsVisual, 
        IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsSendVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var submissionAttr = Module.GetAttributeValue(Xml, "submission");
            if (submissionAttr != null)
            {
                var submissionVisual = ResolveId(submissionAttr);
                if (submissionVisual != null)
                    submissionVisual.Interface<IEventTarget>().DispatchEvent(new XFormsSubmitEvent(submissionVisual).Event);
            }
        }

    }

}
