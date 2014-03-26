using System;
using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    /// <summary>
    /// This form control enables free-form data entry or a user interface component appropriate to the datatype of the
    /// bound node.
    /// </summary>
    [Element("input")]
    public class Input :
        SingleNodeUIBindingElement,
        ISupportsUiCommonAttributes,
        ISupportsIncrementalAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Input()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Input(XElement xml)
            : base(xml)
        {

        }

        protected override void OnAdded(NXObjectEventArgs args)
        {
            base.OnAdded(args);

            this.Interface<IEventTarget>().AddEventHandler("xforms-invalid", false, i =>
            {
                Console.WriteLine(i);
            });

            this.Interface<IEventTarget>().AddEventHandler("xforms-valid", false, i =>
            {
                Console.WriteLine(i);
            });
        }

    }

}
