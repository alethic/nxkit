using System.Xml.Linq;

using NXKit.DOM2.Events;

namespace NXKit.XForms
{

    [Visual("send")]
    public class XFormsSendVisual : 
        XFormsVisual, 
        IActionVisual
    {

        public void Handle(IEvent ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var submissionAttr = Module.GetAttributeValue(Element, "submission");
            if (submissionAttr != null)
            {
                var submissionVisual = ResolveId(submissionAttr);
                if (submissionVisual != null)
                    submissionVisual.DispatchEvent<XFormsSubmitEvent>();
            }
        }

    }

}
