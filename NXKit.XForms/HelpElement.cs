using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element help provides a convenient way to attach help information to a form control.
    /// </summary>
    [Element("help")]
    public class HelpElement :
        SingleNodeUIBindingElement,
        ISupportsCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public HelpElement(XElement element)
            : base(element)
        {

        }

    }

}
