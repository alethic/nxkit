using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("send")]
    public class XFormsSendVisual : XFormsVisual, IActionVisual
    {

        public void Handle(Event ev)
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
