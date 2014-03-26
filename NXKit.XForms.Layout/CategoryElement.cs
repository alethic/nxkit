using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("category")]
    public class CategoryElement :
        Group
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public CategoryElement(XElement xml)
            : base(xml)
        {

        }

    }

}
