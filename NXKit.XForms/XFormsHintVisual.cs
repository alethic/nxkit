using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// The author-optional element hint provides a convenient way to attach hint information to a form control.
    /// </summary>
    [Visual("hint")]
    public class XFormsHintVisual : 
        XFormsSingleNodeBindingVisual,
        ISupportsCommonAttributes
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsHintVisual(XElement element)
            : base(element)
        {

        }

    }

}
