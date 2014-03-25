using System.Xml.Linq;
namespace NXKit.XForms
{

    [Element("select")]
    public class SelectElement :
        SingleNodeBindingElement,
        ISupportsUiCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SelectElement(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

    }

}
