using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("group")]
    public class Group :
        SingleNodeUIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Group()
            : base(Constants.XForms_1_0 + "group")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Group(XElement xml)
            : base(xml)
        {

        }

    }

}
