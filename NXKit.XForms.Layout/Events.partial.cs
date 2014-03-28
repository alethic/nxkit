
using NXKit.DOMEvents;

namespace NXKit.XForms.Layout
{

    public partial class Events
    {

        static readonly EventInfo[] EVENT_INFO = new EventInfo[]
        {
            new EventInfo("xforms-layout-page-next", true, true),
            new EventInfo("xforms-layout-page-previous", true, true),
        };

        public const string PageNext = "xforms-layout-page-next";
        public const string PagePrevious = "xforms-layout-page-previous";

    }

}