using System;
using System.Xml;
using System.Xml.Resolvers;
using System.Xml.Schema;

using NXKit.Xml;
using NXKit.Xml.Schema;

namespace NXKit.XMLEvents.Script.Xml.Schema
{

    /// <summary>
    /// Provides access to the compiled core schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly Uri relativeUri = new Uri("assembly://NXKit.XMLEvents.Script/NXKit/XMLEvents/Script/Xml/Schema/");
        static readonly Lazy<XmlSchemaSet> schemaSet = new Lazy<XmlSchemaSet>(() => LoadXmlSchemaSet(), true);

        /// <summary>
        /// Loads the XML schema with the given XSD path.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        static XmlSchema LoadXmlSchema(Uri uri, XmlResolver resolver)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (uri.IsAbsoluteUri == false)
                throw new ArgumentException(nameof(uri));
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            return XmlSchema.Read(
                XmlReader.Create(
                    uri.ToString(),
                    new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore, XmlResolver = resolver }),
                (s, a) => { if (a.Exception != null) throw a.Exception; });
        }

        /// <summary>
        /// Loads a new <see cref="System.Xml.Schema.XmlSchemaSet"/> containing the ECF-4.0 schema.
        /// </summary>
        /// <returns></returns>
        static XmlSchemaSet LoadXmlSchemaSet()
        {
            var resolver = new XmlPreloadedResolver(new AssemblyResourceXmlResolver(typeof(Xsd).Assembly));
            var builder = new XmlSchemaSetBuilder();
            builder.AddResolver(resolver);
            builder.Add(NXKit.Xml.Schema.Xsd.SchemaSet);
            builder.Add(LoadXmlSchema(new Uri(relativeUri + "xml-script-1.xsd"), resolver));
            return builder.Build();
        }

        /// <summary>
        /// Gets a reference to the ECF schemas as a <see cref="SchemaSet"/>.
        /// </summary>
        public static XmlSchemaSet SchemaSet => schemaSet.Value;

    }

}
