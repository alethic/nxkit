﻿using System;
using System.Diagnostics.Contracts;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a writer that produces a JSON tree from <see cref="NXNode"/> instances.
    /// </summary>
    public class JsonVisualWriter :
        VisualWriter
    {

        readonly JsonWriter writer;
        readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        JsonVisualWriter()
        {
            this.serializer = new JsonSerializer();
            this.serializer.Converters.Add(new XNameJsonConverter());
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public JsonVisualWriter(TextWriter writer)
            : this()
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            this.writer = new Newtonsoft.Json.JsonTextWriter(writer);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        public JsonVisualWriter(Stream stream)
            : this(new StreamWriter(stream))
        {
            Contract.Requires<ArgumentNullException>(stream != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public JsonVisualWriter(JTokenWriter writer)
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
        /// <param name="visual"></param>
        public override void Write(NXNode visual)
        {
            writer.WriteStartObject();

            // write type of visual
            writer.WritePropertyName("Type");
            writer.WriteValue(GetVisualType(visual).FullName);

            // write type inheritance hierarchy.
            var types = GetVisualBaseTypes(visual);
            if (types.Length > 0)
            {
                writer.WritePropertyName("BaseTypes");
                writer.WriteStartArray();
                foreach (var type in types)
                    writer.WriteValue(type.FullName);
                writer.WriteEndArray();
            }

            // write interactive properties
            var properties = GetVisualProperties(visual);
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
                    serializer.Serialize(writer, property.GetValue(visual));

                    // each value gets a version so the client can keep track of changed nodes
                    writer.WritePropertyName("Version");
                    writer.WriteValue(0);

                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }

            // dealing with a content visual
            if (visual is NXElement)
            {
                // write content of visuals
                writer.WritePropertyName("Visuals");
                writer.WriteStartArray();

                // write all children objects
                foreach (var i in ((NXElement)visual).Elements)
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
