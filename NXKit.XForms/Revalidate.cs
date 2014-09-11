using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}revalidate")]
    public class Revalidate :
        ElementExtension,
        IEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Revalidate(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

    }

}
