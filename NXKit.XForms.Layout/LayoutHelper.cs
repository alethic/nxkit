using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.Layout
{

    public static class LayoutHelper
    {

        /// <summary>
        /// Gets the importance of the given visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static Importance GetImportance(ContentVisual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            var attr = visual.Document.GetModule<LayoutModule>().GetAttributeValue(visual.Element, "importance");

            // return value based on string
            switch (attr)
            {
                case "high":
                    return Importance.High;
                case "low":
                    return Importance.Low;
                default:
                    return Importance.Normal;
            }
        }

    }

}
