using System;
using System.ComponentModel.Composition;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Handles exceptions capable of raising events.
    /// </summary>
    [Export(typeof(IExceptionHandler))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Global)]
    public class DOMEventExceptionHandler :
        IExceptionHandler
    {

        public bool HandleException(Exception exception)
        {
            // DOMTargetEventException should raise an event
            var eventException = exception as DOMTargetEventException;
            if (eventException != null)
            {
                var target = eventException.Target.InterfaceOrDefault<EventTarget>();
                if (target == null)
                    return false;

                target.Dispatch(eventException.EventType, eventException.ContextInfo);
                return true;
            }

            return false;
        }

    }

}
