using System.Linq;

namespace NXKit.XForms.Layout
{

    public class LayoutVisual :
        StructuralVisual
    {

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public LayoutModule Module
        {
            get { return Document.GetModule<LayoutModule>(); }
        }

        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

        public string Style
        {
            get { return Document.GetModule<LayoutModule>().GetAttributeValue(Element, "style"); }
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
