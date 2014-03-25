using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("section")]
    public class SectionElement : 
        GroupElement
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SectionElement(XElement xml)
            : base(xml)
        {

        }


    }

}
