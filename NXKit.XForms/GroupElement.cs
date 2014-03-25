using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("group")]
    public class GroupElement :
        SingleNodeBindingElement,
        ISupportsUiCommonAttributes,
        IRelevancyScope
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public GroupElement(XElement element)
            : base(element)
        {

        }

    }

}
