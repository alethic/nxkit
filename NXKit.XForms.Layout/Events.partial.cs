
using NXKit.DOMEvents;

namespace NXKit.XForms.Layout
{

    public partial class Events
    {

        static readonly EventInfo[] EVENT_INFO = new EventInfo[]
        {
            new EventInfo("xforms-layout-step-next", true, true),
            new EventInfo("xforms-layout-step-previous", true, true),
        };

        public const string StepNext = "xforms-layout-step-next";
        public const string StepPrevious = "xforms-layout-step-previous";

    }

}