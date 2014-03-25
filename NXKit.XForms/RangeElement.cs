using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("range")]
    public class RangeElement : 
        SingleNodeBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public RangeElement(XElement element)
            : base(element)
        {

        }

        [Interactive]
        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

        [Interactive]
        public string Start
        {
            get { return Module.GetAttributeValue(Xml, "start"); }
        }

        [Interactive]
        public string End
        {
            get { return Module.GetAttributeValue(Xml, "end"); }
        }

        [Interactive]
        public string Step
        {
            get { return Module.GetAttributeValue(Xml, "step"); }
        }

    }

}
