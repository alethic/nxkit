using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;

namespace NXKit
{

    /// <summary>
    /// Exported document state for serialization and reload.
    /// </summary>
    [Serializable]
    public class NXDocumentState :
        ISerializable
    {

        /// <summary>
        /// Loads the XML from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        static string LoadXml(byte[] buffer)
        {
            var mstm = new MemoryStream(buffer);
            var gstm = new GZipStream(mstm, CompressionMode.Decompress);
            return new StreamReader(gstm, Encoding.UTF8).ReadToEnd();
        }

        /// <summary>
        /// Saves the XML to a buffer.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        static byte[] SaveXml(string xml)
        {
            // compress document to stream
            var mstm = new MemoryStream();
            var gstm = new GZipStream(mstm, CompressionMode.Compress);
            var wrtr = new StreamWriter(gstm, Encoding.UTF8);
            wrtr.Write(xml);
            wrtr.Flush();
            gstm.Close();

            return mstm.ToArray();
        }

        readonly Uri uri;
        readonly string xml;
        readonly int nextElementId;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal NXDocumentState(
            Uri uri,
            string xml,
            int nextElementId)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(xml != null);
            Contract.Requires<ArgumentNullException>(nextElementId >= 0);

            this.uri = uri;
            this.xml = xml;
            this.nextElementId = nextElementId;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXDocumentState(SerializationInfo info, StreamingContext context)
        {
            Contract.Requires<ArgumentNullException>(info != null);

            this.uri = (Uri)info.GetValue("Uri", typeof(Uri));
            this.xml = LoadXml((byte[])info.GetValue("Xml", typeof(byte[])));
            this.nextElementId = info.GetInt32("NextElementId");
        }

        public Uri Uri
        {
            get { return uri; }
        }

        /// <summary>
        /// Saved document.
        /// </summary>
        public string Xml
        {
            get { return xml; }
        }

        /// <summary>
        /// Saved next element Id.
        /// </summary>
        public int NextElementId
        {
            get { return nextElementId; }
        }

        /// <summary>
        /// Serializes the instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Uri", uri);
            info.AddValue("Xml", SaveXml(xml));
            info.AddValue("NextElementId", nextElementId);
        }

    }

}
