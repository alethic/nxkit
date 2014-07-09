using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}load")]
    public class Load :
        ElementExtension,
        IEventHandler
    {

        readonly Lazy<LoadProperties> properties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Load(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.properties = new Lazy<LoadProperties>(() => element.Interface<LoadProperties>());
        }

        public void HandleEvent(Event ev)
        {
            throw new NotImplementedException();
        }

    }

}
