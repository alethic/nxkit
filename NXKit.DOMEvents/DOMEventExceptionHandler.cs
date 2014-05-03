using System;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Handles exceptions capable of raising events.
    /// </summary>
    [ScopeExport(typeof(IExceptionHandler), Scope.Global)]
    public class DOMEventExceptionHandler :
        IExceptionHandler
    {

        public bool HandleException(Exception exception)
        {
            // DOMTargetEventException should raise an event
            var eventException = exception as DOMTargetEventException;
            if (eventException != null)
            {
                var target = eventException.Target.InterfaceOrDefault<INXEventTarget>();
                if (target == null)
                    return false;

                target.DispatchEvent(eventException.EventType, eventException.ContextInfo);
                return true;
            }

            return false;
        }

    }

}
