using NXKit.Web.UI;

namespace NXKit.XForms.Layout
{

    [Visual( "page")]
    public class PageVisual : XFormsGroupVisual, INavigationPageVisual
    {

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
