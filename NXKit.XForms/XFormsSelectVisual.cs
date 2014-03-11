namespace NXKit.XForms
{

    [Visual("select")]
    public class XFormsSelectVisual : 
        XFormsSingleNodeBindingVisual,
        IUiCommon
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
