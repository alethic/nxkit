using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}action")]
    public class Action :
        IAction
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Action(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the associated element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            var actions = element
                .Elements()
                .SelectMany(i => i.Interfaces<IAction>());

            foreach (var action in actions)
                action.Invoke();
        }

    }

}
