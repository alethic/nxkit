using System.Xml.Linq;
namespace NXKit.XForms
{

    [Element("textarea")]
    public class TextArea :
        SingleNodeUIBindingElement,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TextArea(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(this, "incremental") == "true"; }
        }

    }

}
