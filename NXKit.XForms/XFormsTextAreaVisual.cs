namespace NXKit.XForms
{

    [Visual("textarea")]
    public class XFormsTextAreaVisual : XFormsSingleNodeBindingVisual
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
