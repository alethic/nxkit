using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;

using Newtonsoft.Json;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a writer that produces a JSON tree from <see cref="Visual"/> instances.
    /// </summary>
    public class JsonVisualWriter :
        VisualWriter
    {

        readonly JsonWriter writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public JsonVisualWriter(TextWriter writer)
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
        /// Gets the underlying <see cref="JsonWriter"/>.
        /// </summary>
        public JsonWriter JsonWriter
        {
            get { return writer; }
        }

        /// <summary>
        /// Writes the given <see cref="Visual"/> to the underlying output.
        /// </summary>
        /// <param name="visual"></param>
        public override void Write(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            writer.WriteStartObject();

            // write type of visual
            writer.WritePropertyName("_Type");
            writer.WriteValue(visual.GetType().FullName);

            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(visual))
            {
                //writer.WritePropertyName(p.Name);
                //writer.WriteValue(p.GetValue(visual));
            }

            // dealing with a content visual
            if (visual is ContentVisual)
            {
                writer.WritePropertyName("Visuals");
                writer.WriteStartArray();

                // write all children objects
                foreach (var i in ((ContentVisual)visual).Visuals)
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
