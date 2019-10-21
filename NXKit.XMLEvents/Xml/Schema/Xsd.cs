using System;
using System.Collections.Generic;
using System.Xml.Schema;

using NXKit.Xml.Schema;

namespace NXKit.XMLEvents.Xml.Schema
{

    /// <summary>
    /// Provides access to the compiled XML Events schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly EmbeddedXmlHelper helper = new EmbeddedXmlHelper(new Uri("assembly://NXKit.XMLEvents/NXKit/XMLEvents/Xml/Schema/"));

        static Xsd()
        {
            helper.Add(NXKit.Xml.Schema.Xsd.Schemas);
            helper.Add(new Uri("xml-events-1.xsd", UriKind.Relative));
            helper.Add(new Uri("xml-events-attribs-1.xsd", UriKind.Relative));
            helper.Add(new Uri("xml-handlers-1.xsd", UriKind.Relative));
        }

        /// <summary>
        /// Gets a reference to the XML Events schema items.
        /// </summary>
        public static IEnumerable<XmlSchema> Schemas => helper.Schemas;

    }

}
