using System.Xml.Linq;

namespace XEngine.Forms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "send")]
    public class XFormsSendVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode element)
        {
            return new XFormsSendVisual(parent, (XElement)element);
        }

    }

    public class XFormsSendVisual : XFormsVisual, IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        internal XFormsSendVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

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
