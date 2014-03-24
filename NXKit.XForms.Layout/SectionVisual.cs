using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("section")]
    public class SectionVisual : 
        XFormsGroupVisual
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SectionVisual(XElement xml)
            : base(xml)
        {

        }


    }

}
