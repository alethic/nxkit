using System;
using System.Collections.Generic;
using System.Xml.Schema;

using NXKit.Xml.Schema;

namespace NXKit.XMLEvents.Script.Xml.Schema
{

    /// <summary>
    /// Provides access to the XML Events Script schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly EmbeddedXmlHelper helper = new EmbeddedXmlHelper(new Uri("assembly://NXKit.XMLEvents.Script/NXKit/XMLEvents/Script/Xml/Schema/"));

        static Xsd()
        {
            helper.Add(NXKit.Xml.Schema.Xsd.Schemas);
            helper.Add(new Uri("xml-script-1.xsd", UriKind.Relative));
        }

        /// <summary>
        /// Gets a reference to the XML Events Script schema.
        /// </summary>
        public static IEnumerable<XmlSchema> Schemas => helper.Schemas;

    }

}
