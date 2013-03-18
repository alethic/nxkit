using System.Linq;

namespace NXKit.XForms.Web.UI
{

    public static class VisualExtensions
    {

        public static XFormsLabelVisual FindLabelVisual(this StructuralVisual visual)
        {
            return visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
        }

    }

}
