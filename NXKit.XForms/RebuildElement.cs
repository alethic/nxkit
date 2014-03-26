using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [Element("rebuild")]
    public class RebuildElement :
        XFormsElement,
        IActionElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public RebuildElement(XElement xml)
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
                var modelVisual = (ModelElement)ResolveId(modelAttr);
                if (modelVisual != null)
                    modelVisual.OnRebuild();
                else
                {
                    DispatchEvent<BindingExceptionEvent>();
                    return;
                }
            }
        }

    }

}
