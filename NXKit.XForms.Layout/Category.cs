using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("category")]
    public class Category :
        Group
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Category(XElement xml)
            : base(xml)
        {

        }

    }

}
