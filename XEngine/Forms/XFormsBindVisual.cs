using System.Xml.Linq;

namespace XEngine.Forms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "bind")]
    public class XFormsBindVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsBindVisual(parent, (XElement)node);
        }

    }

    public class XFormsBindVisual : XFormsBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsBindVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public override void Refresh()
        {
            // rebuild binding
            Binding = Module.ResolveNodeSetBinding(this);

            base.Refresh();

            // rebuild children
            base.InvalidateChildren();
        }

        /// <summary>
        /// Provides the default evaluation context for child elements, the first element.
        /// </summary>
        public override XFormsEvaluationContext Context
        {
            get { return null; }
        }

    }

}
