using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("group")]
    public class Group :
        SingleNodeUIBindingElement,
        ISupportsUiCommonAttributes,
        IRelevancyScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Group()
            : base()
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
