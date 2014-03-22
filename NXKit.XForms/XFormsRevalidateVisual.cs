using NXKit.DOM2.Events;

namespace NXKit.XForms
{

    [Visual("revalidate")]
    public class XFormsRevalidateVisual : 
        XFormsVisual, 
        IActionVisual
    {

        public void Handle(IEvent ev)
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
                    Module.RevalidateModel(modelVisual);
                else
                {
                    DispatchEvent<XFormsBindingExceptionEvent>();
                    return;
                }
            }
        }

    }

}
