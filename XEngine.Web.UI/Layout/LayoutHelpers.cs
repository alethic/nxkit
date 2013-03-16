using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public static class LayoutHelpers
    {

        /// <summary>
        /// Returns the CSS class to use for the given importance.
        /// </summary>
        /// <param name="importance"></param>
        /// <returns></returns>
        public static string ImportanceCssClass(Importance importance)
        {
            switch (importance)
            {
                case Importance.High:
                    return "Layout__Importance_High";
                case Importance.Low:
                    return "Layout__Importance_Low";
            }

            return null;
        }

    }

}
