using System.Xml.Linq;
namespace NXKit.XForms.Layout
{

    [Element( "page")]
    public class Page :
        Group
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Page(XElement xml)
            : base(xml)
        {

        }

    }

}
