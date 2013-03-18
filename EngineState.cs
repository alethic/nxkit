using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;

namespace NXKit
{

    [Serializable]
    public class EngineState : ISerializable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EngineState()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EngineState(SerializationInfo info, StreamingContext context)
        {
            // decompress document from stream
            var mstm = new MemoryStream((byte[])info.GetValue("1", typeof(byte[])));
            var gstm = new GZipStream(mstm, CompressionMode.Decompress);

            Document = new StreamReader(gstm, Encoding.UTF8).ReadToEnd();
            NextElementId = info.GetInt32("2");
            VisualState = (VisualStateCollection)info.GetValue("4", typeof(VisualStateCollection));
        }

        /// <summary>
        /// Saved engine configuration.
        /// </summary>
        public EngineConfiguration Configuration { get; set; }

        /// <summary>
        /// Saved document.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Saved next element Id.
        /// </summary>
        public int NextElementId { get; set; }

        /// <summary>
        /// Saved module state.
        /// </summary>
        public Dictionary<Type, object> ModuleState { get; set; }

        /// <summary>
        /// Saved visual state.
        /// </summary>
        public VisualStateCollection VisualState { get; set ;}

        /// <summary>
        /// Serializes the instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // compress document to stream
            var mstm = new MemoryStream();
            var gstm = new GZipStream(mstm, CompressionMode.Compress);
            var wrtr = new StreamWriter(gstm, Encoding.UTF8);
            wrtr.Write(Document);
            wrtr.Flush();
            gstm.Close();

            info.AddValue("1", mstm.ToArray());
            info.AddValue("2", NextElementId);
            info.AddValue("4", VisualState);
        }

    }

}
