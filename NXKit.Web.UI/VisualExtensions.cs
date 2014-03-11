using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit.Web.UI
{

    public static class VisualExtensions
    {

        /// <summary>
        /// Returns a rendered text representation of the given text visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static string ToText(this ITextVisual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            var w = new StringWriter();
            visual.WriteText(w);
            return w.ToString();
        }

    }

}
