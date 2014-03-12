using System;
using System.Diagnostics.Contracts;

namespace NXKit.Web.UI
{

    public class FormPage :
        FormNavigation
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        internal FormPage(FormSection parent, INavigationPageVisual visual)
            : base(parent, visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);
        }

    }

}
