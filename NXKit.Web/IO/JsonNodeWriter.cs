using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Util;

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
        /// Writes the given <see cref="XNode"/> to the underlying output.
        /// </summary>
        /// <param name="node"></param>
        public override void Write(XNode node)
        {
            writer.WriteStartObject();

            var items = node.Interfaces()
                .Where(i => i != null)
                .Select(i => new
                {
                    Object = i,
                    Types = TypeDescriptor.GetReflectionType(i)
                        .GetInterfaces()
                        .Concat(TypeDescriptor.GetReflectionType(i)
                            .Recurse(j => j.BaseType))
                        .Where(j => j.GetCustomAttribute<PublicAttribute>(false) != null)
                        .ToList(),
                })
                .Where(i => i.Types.Any())
                .SelectMany(i => i.Types
                    .Select(j => new
                    {
                        Object = i.Object,
                        Type = j,
                        TypeName = j.FullName,
                    }))
                .GroupBy(i => i.TypeName)
                .Select(i => new
                {
                    Type = i.First().Type,
                    TypeName = i.First().TypeName,
                    Object = i.First().Object,
                })
                .Select(i => new
                {
                    Type = i.Type,
                    TypeName = i.TypeName,
                    Object = i.Object,
                    Properties = TypeDescriptor.GetReflectionType(i.Type)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(j => j.DeclaringType == i.Type)
                        .Where(j => j.GetCustomAttribute<PublicAttribute>(false) != null)
                        .GroupBy(j => j.Name)
                        .Select(j => j.First())
                        .ToList(),
                    Methods = TypeDescriptor.GetReflectionType(i.Type)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(j => j.DeclaringType == i.Type)
                        .Where(j => j.GetCustomAttribute<PublicAttribute>(false) != null)
                        .GroupBy(j => j.Name)
                        .Select(j => j.First())
                        .ToList(),
                })
                .Where(i => i.Properties.Any() || i.Methods.Any());

            foreach (var item in items)
            {
                writer.WritePropertyName(item.Type.FullName);
                writer.WriteStartObject();

                foreach (var property in item.Properties)
                {
                    writer.WritePropertyName(property.Name);
                    serializer.Serialize(writer, property.GetValue(item.Object));
                }

                foreach (var method in item.Methods)
                {
                    writer.WritePropertyName("@" + method.Name);
                    writer.WriteStartArray();
                    writer.WriteEndArray();
                }

                writer.WriteEnd();
            }

            // dealing with a content node
            if (node is XElement)
            {
                // write content of nodes
                writer.WritePropertyName("Nodes");
                writer.WriteStartArray();

                // write all children objects
                foreach (var i in ((XElement)node).Nodes())
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
