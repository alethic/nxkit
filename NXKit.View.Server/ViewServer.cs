using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.View.Server.Serialization;
using NXKit.Xml;

namespace NXKit.View.Server
{

    /// <summary>
    /// Hosts a <see cref="Document"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class ViewServer
    {

        readonly IEnumerable<IDocumentStore> stores;
        readonly IEnumerable<IDocumentCache> caches;
        ComposablePartCatalog catalog;
        ExportProvider exports;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ViewServer()
            : this(null, null, store: null, cache: null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <param name="store"></param>
        /// <param name="cache"></param>
        ViewServer(
            ComposablePartCatalog catalog = null,
            ExportProvider exports = null,
            IDocumentStore store = null,
            IDocumentCache cache = null)
            : this(catalog, exports, store != null ? new[] { store } : null, cache != null ? new[] { cache } : null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <param name="stores"></param>
        /// <param name="caches"></param>
        ViewServer(
            ComposablePartCatalog catalog = null,
            ExportProvider exports = null,
            IEnumerable<IDocumentStore> stores = null,
            IEnumerable<IDocumentCache> caches = null)
        {
            this.catalog = catalog;
            this.exports = exports;
            this.stores = (stores ?? new[] { new MemoryDocumentStore() }).Where(i => i != null);
            this.caches = (caches ?? new[] { new MemoryDocumentCache() }).Where(i => i != null);
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
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public ViewMessage Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            // load new document
            return Save(Load(() => Document.Load(uri, catalog, exports)), ViewMessageStatus.Good);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public ViewMessage Load(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            return Load(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Loads the docuemnt from the given <see cref="TextReader"/> into the server.
        /// </summary>
        /// <param name="reader"></param>
        public ViewMessage Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            // load new document
            return Save(Load(() => Document.Load(reader, catalog, exports)), ViewMessageStatus.Good);
        }

        /// <summary>
        /// Loads the docuemnt from the given <see cref="XmlReader"/> into the server.
        /// </summary>
        /// <param name="reader"></param>
        public ViewMessage Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            // load new document
            return Save(Load(() => Document.Load(reader, catalog, exports)), ViewMessageStatus.Good);
        }

        /// <summary>
        /// Loads and executes the object.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ViewMessage Load(ViewMessage message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            // load document from args
            var document = LoadFromMessage(message);
            if (document == null)
                return new ViewMessage(ViewMessageStatus.NotFound);

            try
            {
                Execute(Load(() => document), message);
            }
            catch (Exception)
            {
                return new ViewMessage(ViewMessageStatus.Error);
            }

            // save and return document
            return Save(document, ViewMessageStatus.Good);
        }

        /// <summary>
        /// Raises the DocumentLoaded event.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Document Load(Func<Document> func)
        {
            var document = func();
            OnDocumentLoaded(new DocumentEventArgs(document));
            return document;
        }

        /// <summary>
        /// Raised when the <see cref="Document"/> is loaded.
        /// </summary>
        public event DocumentLoadedEventHandler DocumentLoaded;

        /// <summary>
        /// Raises the DocumentLoaded event.
        /// </summary>
        /// <param name="args"></param>
        void OnDocumentLoaded(DocumentEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            if (DocumentLoaded != null)
                DocumentLoaded(this, args);
        }

        /// <summary>
        /// Raised when the <see cref="Document"/> is unloading.
        /// </summary>
        public event DocumentLoadedEventHandler DocumentUnloading;

        /// <summary>
        /// Raises the DocumentUnloading event.
        /// </summary>
        /// <param name="args"></param>
        void OnDocumentUnloading(DocumentEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            if (DocumentUnloading != null)
                DocumentUnloading(this, args);
        }

        /// <summary>
        /// Gets the MD5 hash of the given data string in text format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetMD5HashText(string data)
        {
            Contract.Requires<ArgumentNullException>(data != null);

            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
            var text = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                text.Append(hash[i].ToString("x2"));
            return text.ToString();
        }

        /// <summary>
        /// Gets the client-side save state as a string.
        /// </summary>
        /// <returns></returns>
        string GetSaveString(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            using (var stream = new MemoryStream())
            using (var encode = new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write))
            using (var deflate = new DeflateStream(encode, CompressionMode.Compress))
            using (var writer = XmlDictionaryWriter.CreateBinaryWriter(deflate))
            {
                document.Save(writer);

                // flush output
                writer.Dispose();
                deflate.Dispose();
                encode.Dispose();

                return Encoding.ASCII.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JToken"/>
        /// </summary>
        /// <returns></returns>
        JToken CreateNodeJObject(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            // serialize document state to data field
            using (var wrt = new JTokenWriter())
            {
                RemoteHelper.GetJson(wrt, document.Root);
                return wrt.Token;
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        object GetNodeObject(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return CreateNodeJObject(document);
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="String"/>.
        /// </summary>
        /// <returns></returns>
        string GetDataString(Document document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return JsonConvert.SerializeObject(GetNodeObject(document));
        }

        /// <summary>
        /// Returns a saved version of the currently loaded document.
        /// </summary>
        /// <returns></returns>
        ViewMessage Save(Document document, ViewMessageStatus status)
        {
            // notify any last minute interested parties
            OnDocumentUnloading(new DocumentEventArgs(document));

            // extract data from document
            var cmds = document.Container.GetExportedValues<ICommandProvider>().SelectMany(i => i.Commands).ToArray();
            var node = GetNodeObject(document);
            var save = GetSaveString(document);
            var hash = GetMD5HashText(save);

            // cache save data
            foreach (var store in stores)
                store.Put(hash, document);
            foreach (var cache in caches)
                cache.Set(hash, save);

            // respond with object containing new save and JSON tree
            return new ViewMessage(status, hash, save, node)
            {
                Commands = cmds,
            };
        }

        /// <summary>
        /// Loads the <see cref="Document"/> from the given save data.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        Document LoadFromSave(string save)
        {
            Contract.Requires<ArgumentNullException>(save != null);

            using (var stm = new MemoryStream(Encoding.ASCII.GetBytes(save)))
            using (var b64 = new CryptoStream(stm, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var cmp = new DeflateStream(b64, CompressionMode.Decompress))
            using (var rdr = XmlDictionaryReader.CreateBinaryReader(cmp, new XmlDictionaryReaderQuotas()))
            {
                return Document.Load(rdr, catalog, exports);
            }
        }

        /// <summary>
        /// Loads the given <see cref="Document"/> from the given hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Document LoadFromHash(string hash)
        {
            //if (hash != null)
            //{
            //    var document = stores.Select(i => i.Get(hash)).FirstOrDefault(i => i != null);
            //    if (document != null)
            //        return document;

            //    var save = caches.Select(i => i.Get(hash)).FirstOrDefault(i => i != null);
            //    if (save != null)
            //        return LoadFromSave(save);
            //}

            return null;
        }

        /// <summary>
        /// Loads the document from the given input args.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Document LoadFromMessage(ViewMessage message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            // load save data
            if (message.Save != null)
                return LoadFromSave(message.Save);

            // load hash data
            if (message.Hash != null)
                return LoadFromHash(message.Hash);

            return null;
        }

        /// <summary>
        /// Executes the commands packed in the arguments.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void Execute(Document document, ViewMessage message)
        {
            Contract.Requires<ArgumentNullException>(message != null);
            Contract.Requires<InvalidOperationException>(document != null);

            if (message.Commands != null)
            {
                foreach (var command in message.Commands)
                {
                    if (command is Commands.Update)
                    {
                        var cmd = (Commands.Update)command;
                        ClientUpdate(document, cmd.NodeId, cmd.Interface, cmd.Property, cmd.Value);
                    }

                    if (command is Commands.Invoke)
                    {
                        var cmd = (Commands.Invoke)command;
                        ClientInvoke(document, cmd.NodeId, cmd.Interface, cmd.Method, cmd.Parameters);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the given property.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="nodeId"></param>
        /// <param name="interface"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void ClientUpdate(Document document, int nodeId, string @interface, string property, JValue value)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentOutOfRangeException>(nodeId > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(@interface));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(property));

            var node = (XNode)document.Xml.ResolveObjectId(nodeId);
            if (node == null)
                return;

            RemoteHelper.Update(node, @interface, property, value);
        }

        /// <summary>
        /// Invokes the given method.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="nodeId"></param>
        /// <param name="interface"></param>
        /// <param name="method"></param>
        /// <param name="params"></param>
        void ClientInvoke(Document document, int nodeId, string @interface, string method, JObject @params)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentOutOfRangeException>(nodeId > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(@interface));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(method));

            var node = (XNode)document.Xml.ResolveObjectId(nodeId);
            if (node == null)
                return;

            RemoteHelper.Invoke(node, @interface, method, @params);
        }

    }

}
