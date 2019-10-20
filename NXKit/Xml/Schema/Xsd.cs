using System;
using System.Xml;
using System.Xml.Resolvers;
using System.Xml.Schema;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides access to the compiled core schema.
    /// </summary>
    public static partial class Xsd
    {

        static readonly Uri relativeUri = new Uri("assembly://NXKit/NXKit/Xml/Schema/");
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
        /// Loads a new <see cref="XmlSchemaSet"/> containing the core schema.
        /// </summary>
        /// <returns></returns>
        static XmlSchemaSet LoadXmlSchemaSet()
        {
            var resolver = new XmlPreloadedResolver(new AssemblyResourceXmlResolver(typeof(Xsd).Assembly));
            var builder = new XmlSchemaSetBuilder();
            builder.AddResolver(resolver);
            builder.Add(LoadXmlSchema(new Uri(relativeUri + "xml.xsd"), resolver));
            builder.Add(LoadXmlSchema(new Uri(relativeUri + "xsdschema.xsd"), resolver));
            return builder.Build();
        }

        /// <summary>
        /// Gets a reference to the core <see cref="XmlSchemaSet"/>.
        /// </summary>
        public static XmlSchemaSet SchemaSet => schemaSet.Value;

    }

}
