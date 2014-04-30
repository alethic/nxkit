using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}input")]
    [ScopeExport(typeof(Input), Scope.Object)]
    public class Input :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Input(XElement element, INXEventTarget eventTarget)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(eventTarget != null);

            // trigger xforms-focus
            eventTarget.AddEventHandler(DOMEvents.Events.DOMFocusIn, evt =>
                eventTarget.DispatchEvent(Events.Focus));
        }

    }

}
