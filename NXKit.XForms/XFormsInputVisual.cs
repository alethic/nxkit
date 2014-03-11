namespace NXKit.XForms
{

    [Visual("input")]
    public class XFormsInputVisual :
        XFormsSingleNodeBindingVisual,
        IUiCommon
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
