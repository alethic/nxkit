using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("section")]
    public class Section :
        Group
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Section(XElement xml)
            : base(xml)
        {

        }

    }

}
