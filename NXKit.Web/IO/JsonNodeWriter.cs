using System;
using System.Diagnostics.Contracts;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a writer that produces a JSON tree from <see cref="NXNode"/> instances.
    /// </summary>
    public class JsonNodeWriter :
        NodeWriter
    {

        readonly JsonWriter writer;
        readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        JsonNodeWriter()
        {
            this.serializer = new JsonSerializer();
            this.serializer.Converters.Add(new XNameJsonConverter());
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public JsonNodeWriter(TextWriter writer)
            : this()
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            this.writer = new Newtonsoft.Json.JsonTextWriter(writer);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        public JsonNodeWriter(Stream stream)
            : this(new StreamWriter(stream))
        {
            Contract.Requires<ArgumentNullException>(stream != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public JsonNodeWriter(JTokenWriter writer)
            : this()
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            this.writer = writer;
        }

        /// <summary>
        /// Gets the underlying <see cref="JsonWriter"/>.
        /// </summary>
        public JsonWriter JsonWriter
        {
            get { return writer; }
        }

        /// <summary>
        /// Writes the given <see cref="NXNode"/> to the underlying output.
        /// </summary>
        /// <param name="node"></param>
        public override void Write(NXNode node)
        {
            writer.WriteStartObject();

            // write type of node
            writer.WritePropertyName("Type");
            writer.WriteValue(GetNodeType(node).FullName);

            // write type inheritance hierarchy.
            var types = GetNodeBaseTypes(node);
            if (types.Length > 0)
            {
                writer.WritePropertyName("BaseTypes");
                writer.WriteStartArray();
                foreach (var type in types)
                    writer.WriteValue(type.FullName);
                writer.WriteEndArray();
            }

            // write interactive properties
            var properties = GetNodeProperties(node);
            if (properties.Length > 0)
            {
                writer.WritePropertyName("Properties");
                writer.WriteStartObject();
                foreach (var property in properties)
                {
                    writer.WritePropertyName(property.Name);
                    writer.WriteStartObject();

                    // serialize value independently to handle custom conversion
                    writer.WritePropertyName("Value");
                    serializer.Serialize(writer, property.GetValue(node));

                    // each value gets a version so the client can keep track of changed nodes
                    writer.WritePropertyName("Version");
                    writer.WriteValue(0);

                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }

            // dealing with a content node
            if (node is NXElement)
            {
                // write content of nodes
                writer.WritePropertyName("Nodes");
                writer.WriteStartArray();

                // write all children objects
                foreach (var i in ((NXElement)node).Nodes())
                    Write(i);

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams.
        /// </summary>
        public override void Flush()
        {
            writer.Flush();
        }

        /// <summary>
        /// Closes this writer and its underlying stream.
        /// </summary>
        public override void Close()
        {
            writer.Close();
        }

        /// <summary>
        /// Closes this writer and its underlying stream.
        /// </summary>
        public override void Dispose()
        {
            Close();
        }

    }

}
