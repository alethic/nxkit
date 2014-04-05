using System;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.DOMEvents;

namespace NXKit.XForms
{

    [NXElementInterface("{http://www.w3.org/2002/xforms}action")]
    public class Action :
        IAction
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Action(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public NXElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the XForms module.
        /// </summary>
        XFormsModule Module
        {
            get { return element.Document.Module<XFormsModule>(); }
        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            var actions = element
                .Elements()
                .OfType<NXElement>()
                .SelectMany(i => i.Interfaces<IAction>());

            foreach (var action in actions)
                Module.InvokeAction(action);
        }

    }

}
