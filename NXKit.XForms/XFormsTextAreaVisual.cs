using System.Xml.Linq;
namespace NXKit.XForms
{

    [Visual("textarea")]
    public class XFormsTextAreaVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsTextAreaVisual(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

    }

}
