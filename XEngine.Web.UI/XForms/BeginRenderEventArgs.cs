using System;
using System.Collections.Generic;

namespace XEngine.Forms.Web.UI.XForms
{

    public class BeginRenderEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal BeginRenderEventArgs()
        {
            CssClasses = new List<string>(4);
        }

        /// <summary>
        /// Gets the set of CSS classes to be added.
        /// </summary>
        public List<string> CssClasses { get; private set; }

    }

}
