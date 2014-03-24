using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("category")]
    public class CategoryVisual :
        XFormsGroupVisual
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public CategoryVisual(XElement xml)
            : base(xml)
        {

        }

    }

}
