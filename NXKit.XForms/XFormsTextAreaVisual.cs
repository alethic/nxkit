namespace NXKit.XForms
{

    [Visual("textarea")]
    public class XFormsTextAreaVisual :
        XFormsSingleNodeBindingVisual,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
