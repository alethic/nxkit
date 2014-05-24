using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NXKit
{

    /// <summary>
    /// Manages the available schema.
    /// </summary>
    internal class SchemaContainer
    {

        /// <summary>
        /// Reference to the hosting engine.
        /// </summary>
        Document Document { get; set; }

        /// <summary>
        /// Set of provided schema packages.
        /// </summary>
        [ImportMany]
        public IEnumerable<SchemaPackage> Packages { get; set; }

        /// <summary>
        /// Initializes a new static instance.
        /// </summary>
        internal SchemaContainer(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            Document = document;

            // initialize .Net schema
            {
                var settings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    ValidationType = ValidationType.None,
                };

                var schemaStreams = Packages
                    .SelectMany(i => i.Namespaces)
                    .Where(i => i != null)
                    .Select(i => ResolveSchema(i))
                    .Where(i => i != null)
                    .Select(i => OpenSchema(i))
                    .Where(i => i != null);

                // initialize static schema set
                SchemaSet = new XmlSchemaSet();
                SchemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
                SchemaSet.XmlResolver = new XmlResolver(this);
                foreach (var stream in schemaStreams)
                    SchemaSet.Add(null, XmlReader.Create(stream, settings));

                // identify imported duplicates
                var duplicateSchemas = new List<XmlSchema>();
                var schemas = new List<XmlSchema>(SchemaSet.Schemas().Cast<XmlSchema>());
                for (int i = 0; i < schemas.Count; i++)
                    if (schemas[i].SourceUri == "")
                        duplicateSchemas.Add(schemas[i]);
                    else
                    {
                        for (int j = i + 1; j < schemas.Count; j++)
                            if (schemas[i].SourceUri == schemas[j].SourceUri)
                                duplicateSchemas.Add(schemas[j]);
                    }

                // remove duplicates
                foreach (var schema in duplicateSchemas)
                    SchemaSet.Remove(schema);

                // compile schema set
                SchemaSet.Compile();
            }
        }

        /// <summary>
        /// Gets a reference to the compiled schema set.
        /// </summary>
        public XmlSchemaSet SchemaSet { get; private set; }

        /// <summary>
        /// Resolves the location of the given schema.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public string ResolveSchema(XNamespace ns)
        {
            Contract.Requires<ArgumentNullException>(Packages != null);
            return Packages.Select(i => i.ResolveSchema(ns)).Where(i => i != null).FirstOrDefault();
        }

        /// <summary>
        /// Opens a new stream for the given schema location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public Stream OpenSchema(string location)
        {
            Contract.Requires<ArgumentNullException>(location != null);
            Contract.Requires<ArgumentNullException>(Packages != null);
            return Packages.Select(i => i.OpenSchema(location)).Where(i => i != null).FirstOrDefault();
        }

        /// <summary>
        /// Invoked when there's an issue validating a document against the schema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
                throw args.Exception;
        }

        /// <summary>
        /// Provides an <see cref="XmlResolver"/> implementation for users of ISIS.Forms.
        /// </summary>
        class XmlResolver : XmlUrlResolver
        {

            SchemaContainer schema;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="schema"></param>
            internal XmlResolver(SchemaContainer schema)
            {
                Contract.Requires<ArgumentNullException>(schema != null);
                this.schema = schema;
            }

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                Contract.Assume(schema != null);
                Contract.Assume(schema.Packages != null);
                return schema.ResolveSchema(absoluteUri.ToString());
            }

        }

    }

}