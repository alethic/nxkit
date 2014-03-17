namespace NXKit.XForms
{

    [Visual("range")]
    public class XFormsRangeVisual : 
        XFormsSingleNodeBindingVisual
    {

        [Interactive]
        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

        [Interactive]
        public string Start
        {
            get { return Module.GetAttributeValue(Element, "start"); }
        }

        [Interactive]
        public string End
        {
            get { return Module.GetAttributeValue(Element, "end"); }
        }

        [Interactive]
        public string Step
        {
            get { return Module.GetAttributeValue(Element, "step"); }
        }

    }

}
