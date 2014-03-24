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
        public static Importance GetImportance(NXElement visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            var attr = visual.Document.Module<LayoutModule>().GetAttributeValue(visual.Xml, "importance");

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
