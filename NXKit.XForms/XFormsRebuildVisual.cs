using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Visual("rebuild")]
    public class XFormsRebuildVisual :
        XFormsVisual, 
        IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsRebuildVisual(NXElement parent, XElement element)
            : base(parent, element)
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
