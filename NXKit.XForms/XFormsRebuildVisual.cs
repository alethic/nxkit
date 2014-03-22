using NXKit.DOM.Events;

namespace NXKit.XForms
{

    [Visual("rebuild")]
    public class XFormsRebuildVisual :
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
                    Module.RebuildModel(modelVisual);
                else
                {
                    DispatchEvent<XFormsBindingExceptionEvent>();
                    return;
                }
            }
        }

    }

}
