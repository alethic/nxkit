using System.Collections.Generic;

namespace NXKit.DOMEvents
{

    public class DOMEventsModule :
        Module
    {

        public override IEnumerable<object> GetInterfaces(NXObject obj)
        {
            foreach (var i in base.GetInterfaces(obj))
                yield return i;

            if (obj is NXNode)
                yield return new EventTarget((NXNode)obj);

            if (obj is NXDocument)
                yield return new DocumentEvent((NXDocument)obj);
        }

    }

}
