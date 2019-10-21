using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides convience methods for building a set of XML schema.
    /// </summary>
    public class XmlSchemaSetBuilder
    {

        readonly List<XmlSchema> buffer = new List<XmlSchema>();
        readonly List<XmlResolver> resolvers = new List<XmlResolver>();

        /// <summary>
        /// Adds a <see cref="XmlSchema"/> to the builder.
        /// </summary>
        /// <param name="schema"></param>
        public XmlSchemaSetBuilder Add(XmlSchema schema)
        {
            if (schema is null)
                throw new ArgumentNullException(nameof(schema));

            buffer.Add(schema);

            return this;
        }

        /// <summary>
        /// Adds a set of <see cref="XmlSchema"/>s to the builder.
        /// </summary>
        /// <param name="schemas"></param>
        public XmlSchemaSetBuilder Add(IEnumerable<XmlSchema> schemas)
        {
            if (schemas is null)
                throw new ArgumentNullException(nameof(schemas));

            buffer.AddRange(schemas);

            return this;
        }

        /// <summary>
        /// Adds a <see cref="XmlSchemaSet"/> to the builder.
        /// </summary>
        /// <param name="schemaSet"></param>
        public XmlSchemaSetBuilder Add(XmlSchemaSet schemaSet)
        {
            if (schemaSet is null)
                throw new ArgumentNullException(nameof(schemaSet));

            foreach (var schema in schemaSet.Schemas().Cast<XmlSchema>())
                Add(schema);

            return this;
        }

        /// <summary>
        /// Adds a <see cref="XmlResolver"/> to the builder.
        /// </summary>
        /// <param name="resolver"></param>
        public XmlSchemaSetBuilder AddResolver(XmlResolver resolver)
        {
            if (resolver is null)
                throw new ArgumentNullException(nameof(resolver));

            resolvers.Add(resolver);

            return this;
        }

        /// <summary>
        /// Builds and compiles a new <see cref="XmlSchemaSet"/> from the configured schemas.
        /// </summary>
        /// <returns></returns>
        public XmlSchemaSet Build(ValidationEventHandler validationEventHandler = null)
        {
            // temporary buffer with no validation nor resolvers
            var buff = new XmlSchemaSet();
            buff.XmlResolver = null;
            buff.CompilationSettings.EnableUpaCheck = false;
            buff.ValidationEventHandler += (s, a) => { };

            // add from merge results
            foreach (var schema in buffer)
                buff.Add(schema);

            // build new schema set with resolver and compile, but ignore errors
            var temp = new XmlSchemaSet();
            temp.XmlResolver = null;
            temp.CompilationSettings.EnableUpaCheck = false;
            temp.ValidationEventHandler += (s, a) => { };
            temp.Add(buff);
            temp.Compile();

            void ValidationEventHandler(object sender, ValidationEventArgs args)
            {
                if (validationEventHandler != null)
                    validationEventHandler(sender, args);
                else if (args.Exception != null)
                    throw args.Exception;
            }

            // build new schema set from results and compile, this time with errors
            var rslt = new XmlSchemaSet();
            rslt.XmlResolver = null;
            rslt.CompilationSettings.EnableUpaCheck = false;
            rslt.ValidationEventHandler += ValidationEventHandler;
            rslt.Add(temp);
            rslt.Compile();

            return rslt;
        }

    }

}
