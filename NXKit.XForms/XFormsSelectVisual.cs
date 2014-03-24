using System.Xml.Linq;
namespace NXKit.XForms
{

    [Visual("select")]
    public class XFormsSelectVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XFormsSelectVisual(XElement element)
            : base(element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Xml, "incremental") == "true"; }
        }

    }

}
