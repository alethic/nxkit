using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "item")]
    public class XFormsItemVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsItemVisual(parent, (XElement)node);
        }

    }

    public class XFormsItemVisual : XFormsVisual
    {

        private bool labelVisualCached;
        private XFormsLabelVisual labelVisual;
        private bool selectableCached;
        private ISelectableVisual selectable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsItemVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets the <see cref="XFormsLabelVisual"/> for this item.
        /// </summary>
        public XFormsLabelVisual LabelVisual
        {
            get
            {
                if (!labelVisualCached)
                {
                    labelVisual = Children.OfType<XFormsLabelVisual>().SingleOrDefault();
                    labelVisualCached = true;
                }

                return labelVisual;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISelectable"/> for this item.
        /// </summary>
        public ISelectableVisual Selectable
        {
            get
            {
                if (!selectableCached)
                {
                    selectable = Children.OfType<ISelectableVisual>().SingleOrDefault();
                    selectableCached = true;
                }

                return selectable;
            }
        }

    }

}
