using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Provides convience methods for building a set of XML schema.
    /// </summary>
    public class XmlSchemaSetBuilder
    {

        readonly XmlSchemaSet buffer;
        readonly List<XmlResolver> resolvers = new List<XmlResolver>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XmlSchemaSetBuilder()
        {
            buffer = new XmlSchemaSet();
            buffer.ValidationEventHandler += (s, a) => { };
            buffer.XmlResolver = null;
        }

        /// <summary>
        /// Adds a <see cref="XmlSchema"/> to the builder.
        /// </summary>
        /// <param name="schema"></param>
        public void Add(XmlSchema schema)
        {
            if (schema is null)
                throw new System.ArgumentNullException(nameof(schema));

            buffer.Add(schema);
        }

        /// <summary>
        /// Adds a <see cref="XmlSchemaSet"/> to the builder.
        /// </summary>
        /// <param name="schemaSet"></param>
        public void Add(XmlSchemaSet schemaSet)
        {
            if (schemaSet is null)
                throw new System.ArgumentNullException(nameof(schemaSet));

            buffer.Add(schemaSet);
        }

        /// <summary>
        /// Adds a <see cref="XmlResolver"/> to the builder.
        /// </summary>
        /// <param name="resolver"></param>
        public void AddResolver(XmlResolver resolver)
        {
            if (resolver is null)
                throw new System.ArgumentNullException(nameof(resolver));

            resolvers.Add(resolver);
        }

        /// <summary>
        /// Builds and compiles a new <see cref="XmlSchemaSet"/> from the configured schemas.
        /// </summary>
        /// <returns></returns>
        public XmlSchemaSet Build()
        {
            // build new schema set with resolver and compile, but ignore errors
            var temp = new XmlSchemaSet();
            temp.XmlResolver = new AggregateXmlResolver(resolvers);
            temp.ValidationEventHandler += (s, a) => { };
            temp.Add(buffer);
            temp.Compile();

            // build new schema set from results and compile, this time with errors
            var rslt = new XmlSchemaSet();
            rslt.XmlResolver = null;
            rslt.ValidationEventHandler += (s, a) => { if (a.Exception != null) throw a.Exception; };
            rslt.Add(temp);
            rslt.Compile();

            return rslt;
        }

    }

}
