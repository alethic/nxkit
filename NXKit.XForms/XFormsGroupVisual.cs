using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("group")]
    public class XFormsGroupVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        IRelevancyScope
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsGroupVisual(XElement element)
            : base(element)
        {

        }

    }

}
