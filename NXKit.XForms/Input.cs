using System;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}input")]
    public class Input :
        ElementExtension,
        IEventDefaultAction
    {

        readonly IExport<EventTarget> eventTarget;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="eventTarget"></param>
        public Input(
            XElement element,
            IExport<EventTarget> eventTarget)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.eventTarget = eventTarget ?? throw new ArgumentNullException(nameof(eventTarget));
        }

        void IEventDefaultAction.DefaultAction(Event evt)
        {
            if (evt.Type == DOMEvents.Events.DOMFocusIn)
                eventTarget.Value.Dispatch(Events.Focus);
        }

    }

}
