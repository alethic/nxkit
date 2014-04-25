using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.Diagnostics;
using NXKit.IO;
using NXKit.Xml;

namespace NXKit.XInclude
{

    [Interface("{http://www.w3.org/2001/XInclude}include")]
    public class Include :
        ElementExtension,
        IOnInit
    {

        readonly ITraceService trace;
        readonly IIOService io;
        readonly IncludeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Include(XElement element, ITraceService trace, IIOService io)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(trace != null);
            Contract.Requires<ArgumentNullException>(io != null);

            this.trace = trace;
            this.io = io;
            this.attributes = new IncludeAttributes(element);
        }

        public void Init()
        {
            var uri = new Uri(attributes.Href ?? "", UriKind.RelativeOrAbsolute);
            if (Element.GetBaseUri() != null && !uri.IsAbsoluteUri)
                uri = new Uri(Element.GetBaseUri(), uri);

            trace.Information("XInclude: {0}", uri);

            var xml = XDocument.Load(IOXmlReader.Create(io, uri));
            if (xml != null)
            {
                // annotate element and replace self in graph
                var element = new XElement(xml.Root);
                element.SetBaseUri(xml.BaseUri);
                Element.AddBeforeSelf(element);
                Element.Remove();

                trace.Information("XInclude: {0} replaced node {1}", element, Element);
            }
        }

    }

}
