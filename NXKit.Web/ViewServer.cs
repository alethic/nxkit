using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NXKit.Web.Serialization;
using NXKit.Web.UI;

namespace NXKit.Web
{

    /// <summary>
    /// Hosts a <see cref="NXDocumentHost"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class ViewServer
    {

        ComposablePartCatalog catalog;
        ExportProvider exports;
        NXDocumentHost document;
        LinkedList<string> scripts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ViewServer()
        {

        }

        /// <summary>
        /// Gets or sets the additional set of exports to introduce to the document.
        /// </summary>
        public ExportProvider Exports
        {
            get { return exports; }
            set { exports = value; }
        }

        /// <summary>
        /// Gets or sets the additional set of parts to introduce to the document.
        /// </summary>
        public ComposablePartCatalog Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }

        /// <summary>
        /// Gets the currently hosted <see cref="NXDocumentHost"/>.
        /// </summary>
        public NXDocumentHost Document
        {
            get { return document; }
        }

        /// <summary>
        /// Raised when the <see cref="NXDocumentHost"/> is loaded.
        /// </summary>
        public event HostLoadedEventHandler HostLoaded;

        /// <summary>
        /// Raises the HostLoaded event.
        /// </summary>
        /// <param name="args"></param>
        void OnHostLoaded(HostEventArgs args)
        {
            if (HostLoaded != null)
                HostLoaded(this, args);
        }

        /// <summary>
        /// Raised when the <see cref="NXDocumentHost"/> is unloading.
        /// </summary>
        public event HostLoadedEventHandler HostUnloading;

        /// <summary>
        /// Raises the HostUnloading event.
        /// </summary>
        /// <param name="args"></param>
        void OnHostUnloading(HostEventArgs args)
        {
            if (HostUnloading != null)
                HostUnloading(this, args);
        }

        /// <summary>
        /// Registers the given script snippet for execution upon completion of the current page request or client
        /// callback.
        /// </summary>
        /// <param name="script"></param>
        public void RegisterScript(string script)
        {
            (scripts ?? (scripts = new LinkedList<string>())).AddLast(script);
        }

        /// <summary>
        /// Loads the document host from the given saved state.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        NXDocumentHost LoadFromString(string save)
        {
            Contract.Requires<ArgumentNullException>(save != null);

            using (var stm = new MemoryStream(Encoding.ASCII.GetBytes(save)))
            using (var b64 = new CryptoStream(stm, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var cmp = new DeflateStream(b64, CompressionMode.Decompress))
            using (var rdr = XmlDictionaryReader.CreateBinaryReader(cmp, new XmlDictionaryReaderQuotas()))
            {
                return NXDocumentHost.Load(rdr, catalog, exports);
            }
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public void Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            document = NXDocumentHost.Load(uri, catalog, exports);
            OnHostLoaded(HostEventArgs.Empty);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public void Load(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            Load(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Gets the client-side save state as a string.
        /// </summary>
        /// <returns></returns>
        string CreateSaveString()
        {
            using (var stm = new MemoryStream())
            using (var b64 = new CryptoStream(stm, new ToBase64Transform(), CryptoStreamMode.Write))
            using (var cmp = new DeflateStream(b64, CompressionMode.Compress))
            using (var xml = XmlDictionaryWriter.CreateBinaryWriter(cmp))
            {
                document.Save(xml);

                // flush output
                xml.Dispose();
                cmp.Dispose();
                b64.Dispose();

                return Encoding.ASCII.GetString(stm.ToArray());
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JToken"/>
        /// </summary>
        /// <returns></returns>
        JToken CreateNodeJObject()
        {
            // serialize document state to data field
            using (var wrt = new JTokenWriter())
            {
                RemoteHelper.GetJson(wrt, document.Root);
                return wrt.Token;
            }
        }

        /// <summary>
        /// Gets the client-side message data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken CreateMessagesJObject()
        {
            return JArray.FromObject(document.Container.GetExportedValue<TraceSink>().Messages);
        }

        /// <summary>
        /// Gets the client-side script data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken CreateScriptsJObject()
        {
            return new JArray(scripts);
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken CreateDataJObject()
        {
            return new JObject(
                new JProperty("Node", CreateNodeJObject()),
                new JProperty("Messages", CreateMessagesJObject()),
                new JProperty("Scripts", CreateScriptsJObject()));
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="String"/>.
        /// </summary>
        /// <returns></returns>
        string CreateDataString()
        {
            return JsonConvert.SerializeObject(CreateDataJObject());
        }

    }

}
