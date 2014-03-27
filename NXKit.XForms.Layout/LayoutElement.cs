using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Abstract class for layout <see cref="NXNode"/>s.
    /// </summary>
    public abstract class LayoutElement :
        NXElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public LayoutElement(XElement element)
            : base(element)
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

    }

}
