using System.Collections.Generic;

using NXKit.Composition;
using NXKit.DOMEvents;

namespace NXKit.XForms.Layout
{

    [Export(typeof(IEventInfoTable))]
    public partial class Events :
        IEventInfoTable
    {

        public IEnumerable<EventInfo> GetEventInfos()
        {
            return EVENT_INFO;
        }

    }

}
