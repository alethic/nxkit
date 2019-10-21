using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides assistance in resolving a <see cref="XmlSchemaSet"/> from embedded resources.
    /// </summary>
    public class EmbeddedXmlHelper
    {

        readonly Uri baseUri;
        readonly XmlResolver resolver = new AssemblyResourceXmlResolver(AppDomain.CurrentDomain.GetAssemblies());
        readonly HashSet<XmlSchema> schemas = new HashSet<XmlSchema>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseUri"></param>
        public EmbeddedXmlHelper(Uri baseUri = null)
        {
            this.baseUri = baseUri;
        }

        /// <summary>
        /// Adds an existing schema set.
        /// </summary>
        /// <param name="input"></param>
        public void Add(XmlSchema input) => schemas.Add(input);

        /// <summary>
        /// Adds an existing schema set.
        /// </summary>
        /// <param name="input"></param>
        public void Add(IEnumerable<XmlSchema> input)
        {
            foreach (var schema in input)
                schemas.Add(schema);
        }

        /// <summary>
        /// Adds a URI to be processed.
        /// </summary>
        /// <param name="input"></param>
        public void Add(Uri input) => schemas.Add(LoadXmlSchema(input.IsAbsoluteUri ? input : new Uri(baseUri, input)));

        /// <summary>
        /// Loads the XML schema with the given XSD path.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        XmlSchema LoadXmlSchema(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (uri.IsAbsoluteUri == false)
                throw new ArgumentException(nameof(uri));

            return XmlSchema.Read(
                XmlReader.Create(
                    uri.ToString(),
                    new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore, ValidationType = ValidationType.None, XmlResolver = resolver }),
                (s, a) => { if (a.Exception != null) throw a.Exception; });
        }

        /// <summary>
        /// Produces the final schema set.
        /// </summary>
        public IEnumerable<XmlSchema> Schemas => schemas;

    }

}
