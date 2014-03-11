namespace NXKit.XForms
{

    [Visual("select")]
    public class XFormsSelectVisual : 
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
