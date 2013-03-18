namespace NXKit.XForms
{

    [Visual("input")]
    public class XFormsInputVisual : XFormsSingleNodeBindingVisual
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
