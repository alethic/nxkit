using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Handles exceptions capable of raising events.
    /// </summary>
    [ScopeExport(typeof(IExceptionHandler), Scope.Host)]
    public class DOMEventExceptionHandler :
        IExceptionHandler
    {

        readonly XDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        [ImportingConstructor]
        public DOMEventExceptionHandler(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

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
