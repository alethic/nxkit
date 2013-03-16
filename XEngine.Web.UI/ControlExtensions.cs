using System.Collections.Generic;
using System.Web.UI;

namespace XEngine.Forms.Web.UI
{

    public static class ControlExtensions
    {

        public static IEnumerable<Control> Ascendents(this Control self)
        {
            var ctl = self.Parent;
            while (ctl != null)
            {
                yield return ctl;
                ctl = ctl.Parent;
            }
        }

    }

}
