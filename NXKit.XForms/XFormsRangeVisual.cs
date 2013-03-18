namespace NXKit.XForms
{

    [Visual("range")]
    public class XFormsRangeVisual : XFormsSingleNodeBindingVisual
    {

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

        public string Start
        {
            get { return Module.GetAttributeValue(Element, "start"); }
        }

        public string End
        {
            get { return Module.GetAttributeValue(Element, "end"); }
        }

        public string Step
        {
            get { return Module.GetAttributeValue(Element, "step"); }
        }

    }

}
