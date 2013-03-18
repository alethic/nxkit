using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("refresh")]
    public class XFormsRefreshVisual : XFormsVisual, IActionVisual
    {

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var modelAttr = Module.GetAttributeValue(Element, "model");
            if (modelAttr != null)
            {
                var modelVisual = (XFormsModelVisual)ResolveId(modelAttr);
                if (modelVisual != null)
                    Module.RefreshModel(modelVisual);
                else
                {
                    DispatchEvent<XFormsBindingExceptionEvent>();
                    return;
                }
            }
        }

    }

}
