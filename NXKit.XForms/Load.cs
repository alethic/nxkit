using System;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}load")]
    public class Load :
        ElementExtension,
        IEventHandler
    {

        readonly LoadProperties properties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        public Load(
            XElement element,
            LoadProperties properties)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public void HandleEvent(Event ev)
        {
            throw new NotImplementedException();
        }

    }

}
