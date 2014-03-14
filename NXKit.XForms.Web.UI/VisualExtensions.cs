using System.Linq;

namespace NXKit.XForms.Web.UI
{

    public static class VisualExtensions
    {

        public static XFormsLabelVisual FindLabelVisual(this ContentVisual visual)
        {
            return visual.Visuals.OfType<XFormsLabelVisual>().FirstOrDefault();
        }

    }

}
