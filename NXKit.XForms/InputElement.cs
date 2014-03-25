using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Element("input")]
    public class InputElement :
        SingleNodeBindingElement,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public InputElement(XElement element)
            : base(element)
        {

        }

    }

}
