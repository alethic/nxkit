using System.Collections.Generic;

namespace NXKit.DOMEvents
{

    public class DOMEventsModule :
        Module
    {

        public override IEnumerable<object> GetInterfaces(Visual visual)
        {
            yield return new EventTarget(visual);
        }

    }

}
