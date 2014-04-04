using System.Xml.Linq;
namespace NXKit.XForms
{

    [Element("select")]
    public class Select :
        SingleNodeUIBindingElement,
        ISupportsUiCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

    }

}
