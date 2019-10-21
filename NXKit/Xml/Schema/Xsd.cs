using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides access to the compiled core schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly EmbeddedXmlHelper helper = new EmbeddedXmlHelper(new Uri("assembly://NXKit/NXKit/Xml/Schema/"));

        static Xsd()
        {
            helper.Add(new Uri("xml.xsd", UriKind.Relative));
            helper.Add(new Uri("XMLSchema.xsd", UriKind.Relative));
            helper.Add(new Uri("XMLSchema-instance.xsd", UriKind.Relative));
        }

        /// <summary>
        /// Gets a reference to the core <see cref="XmlSchemaSet"/>.
        /// </summary>
        public static IEnumerable<XmlSchema> Schemas => helper.Schemas;

    }

}
