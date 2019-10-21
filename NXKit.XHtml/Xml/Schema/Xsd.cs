using System;
using System.Collections.Generic;
using System.Xml.Schema;

using NXKit.Xml.Schema;

namespace NXKit.XHtml.Xml.Schema
{

    /// <summary>
    /// Provides access to the XHTML schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly EmbeddedXmlHelper helper = new EmbeddedXmlHelper(new Uri("assembly://NXKit.XHtml/NXKit/XHtml/Xml/Schema/"));

        static Xsd()
        {
            helper.Add(NXKit.Xml.Schema.Xsd.Schemas);
            helper.Add(new Uri("xhtml1-strict.xsd", UriKind.Relative));
        }

        /// <summary>
        /// Gets a reference to the XHTML schemas.
        /// </summary>
        public static IEnumerable<XmlSchema> Schemas => helper.Schemas;

    }

}
