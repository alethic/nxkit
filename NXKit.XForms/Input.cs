using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Xml;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}input")]
    public class Input :
        ElementExtension,
        IOnLoad
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Input(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public void Load()
        {
            var eventTarget = Element.Interface<INXEventTarget>();
            if (eventTarget == null)
                throw new NullReferenceException();

            eventTarget.AddEventHandler(DOMEvents.Events.DOMFocusIn, evt =>
                eventTarget.DispatchEvent(Events.Focus));
        }

    }

}
