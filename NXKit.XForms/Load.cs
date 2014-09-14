using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}load")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Load :
        ElementExtension,
        IEventHandler
    {

        readonly Extension<LoadProperties> properties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        [ImportingConstructor]
        public Load(
            XElement element,
            Extension<LoadProperties> properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.properties = properties;
        }

        public void HandleEvent(Event ev)
        {
            throw new NotImplementedException();
        }

    }

}
