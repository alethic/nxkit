using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Abstract class for layout <see cref="NXNode"/>s.
    /// </summary>
    public abstract class LayoutVisual :
        NXElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public LayoutVisual(XElement element)
            : base(element)
        {

        }
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public LayoutVisual(NXElement parent, XElement element)
            : base(parent, element)
        {

        }

        /// <summary>
        /// Gets a reference to the <see cref="XFormsModule"/> instance.
        /// </summary>
        public LayoutModule Module
        {
            get { return Document.Module<LayoutModule>(); }
        }

        public override string Id
        {
            get { return Document.GetElementId(Xml); }
        }

        public string Style
        {
            get { return Document.Module<LayoutModule>().GetAttributeValue(Xml, "style"); }
        }

        /// <summary>
        /// Gets whether or not this visual is enabled.
        /// </summary>
        public bool Relevant
        {
            get
            {
                if (this.Ancestors().OfType<IRelevancyScope>().Any(i => !i.Relevant))
                    return false;

                return true;
            }
        }

    }

}
