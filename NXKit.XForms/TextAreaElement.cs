using System.Xml.Linq;
namespace NXKit.XForms
{

    [Element("textarea")]
    public class TextAreaElement :
        SingleNodeUIBindingElement,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TextAreaElement(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

    }

}
