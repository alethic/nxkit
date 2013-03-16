using System.Linq;
using System.Xml.Linq;

using ISIS.Forms.XForms;

namespace ISIS.Forms.Layout
{

    public class LayoutVisual : StructuralVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        protected LayoutVisual(IFormProcessor form, StructuralVisual parent, XElement element)
            : base(form, parent, element)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        protected LayoutVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public LayoutModule Module
        {
            get { return Form.GetModule<LayoutModule>(); }
        }

        public override string Id
        {
            get { return Form.GetElementId(Element); }
        }

        public string Style
        {
            get { return Form.GetModule<LayoutModule>().GetAttributeValue(Element, "style"); }
        }

        /// <summary>
        /// Gets whether or not this visual is enabled.
        /// </summary>
        public bool Relevant
        {
            get
            {
                if (Ascendants().OfType<IRelevancyScope>().Any(i => !i.Relevant))
                    return false;

                return true;
            }
        }

    }

}
