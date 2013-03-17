using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NXKit
{

    /// <summary>
    /// Provides access to a static <see cref="JavaSchema"/> instance for working with ISIS forms.
    /// </summary>
    internal static class FormSchema
    {

        /// <summary>
        /// Hosts the MEF loaded schema packages.
        /// </summary>
        private class PackageContainer
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public PackageContainer()
            {
                FormProcessor.container.SatisfyImportsOnce(this);
            }

            [ImportMany(typeof(SchemaPackage))]
            public IEnumerable<SchemaPackage> Packages { get; set; }

        }

        private static readonly PackageContainer container = new PackageContainer();

        /// <summary>
        /// Initializes a new static instance.
        /// </summary>
        static FormSchema()
        {
            // initialize .Net schema
            {
                var settings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    ValidationType = ValidationType.None,
                };

                var schemaStreams = container.Packages
                    .SelectMany(i => i.Namespaces)
                    .Where(i => i != null)
                    .Select(i => ResolveSchema(i))
                    .Where(i => i != null)
                    .Select(i => OpenSchema(i))
                    .Where(i => i != null);

                // initialize static schema set
                SchemaSet = new XmlSchemaSet();
                SchemaSet.ValidationEventHandler += SchemaSet_ValidationEventHandler;
                SchemaSet.XmlResolver = new XmlResolver();
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
        public static XmlSchemaSet SchemaSet { get; private set; }

        /// <summary>
        /// Resolves the location of the given schema.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string ResolveSchema(XNamespace ns)
        {
            return container.Packages.Select(i => i.ResolveSchema(ns)).Where(i => i != null).FirstOrDefault();
        }

        /// <summary>
        /// Opens a new stream for the given schema location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Stream OpenSchema(string location)
        {
            return container.Packages.Select(i => i.OpenSchema(location)).Where(i => i != null).FirstOrDefault();
        }

        /// <summary>
        /// Invoked when there's an issue validating a document against the schema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void SchemaSet_ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
                throw args.Exception;
        }

        /// <summary>
        /// Provides an <see cref="XmlResolver"/> implementation for users of ISIS.Forms.
        /// </summary>
        public class XmlResolver : XmlUrlResolver
        {

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                return ResolveSchema(absoluteUri.ToString());
            }

        }

    }

}