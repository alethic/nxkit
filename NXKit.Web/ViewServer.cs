using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Web.Serialization;
using NXKit.Web.UI;
using NXKit.Xml;

namespace NXKit.Web
{

    /// <summary>
    /// Hosts a <see cref="Document"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class ViewServer :
        IDisposable
    {

        ComposablePartCatalog catalog;
        ExportProvider exports;
        ICache cache;
        Lazy<Document> document;
        ViewResponseCode code;
        LinkedList<string> scripts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ViewServer()
            : this(null, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="exports"></param>
        /// <param name="cache"></param>
        ViewServer(ComposablePartCatalog catalog = null, ExportProvider exports = null, ICache cache = null)
        {
            this.catalog = catalog;
            this.exports = exports;
            this.cache = cache ?? new DefaultMemoryCache();
            this.code = ViewResponseCode.Unknown;
            this.scripts = new LinkedList<string>();
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
        /// Gets the currently hosted <see cref="Document"/>.
        /// </summary>
        public Document Document
        {
            get { return document != null ? document.Value : null; }
        }

        /// <summary>
        /// Releases the current document.
        /// </summary>
        void Release()
        {
            if (document != null)
            {
                OnDocumentUnloading(DocumentEventArgs.Empty);
                document = null;
                code = ViewResponseCode.Unknown;
            }
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
            Contract.Requires<InvalidOperationException>(Document != null);

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
            Contract.Requires<InvalidOperationException>(Document != null);

            if (DocumentUnloading != null)
                DocumentUnloading(this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlTemplateInfo> GetHtmlTemplates()
        {
            if (document != null)
                return document.Value.Container
                    .GetExportedValues<IHtmlTemplateProvider>()
                    .SelectMany(i => i.GetTemplates());
            else
                return Enumerable.Empty<HtmlTemplateInfo>();
        }

        /// <summary>
        /// Registers the given script snippet for execution upon return to the client.
        /// </summary>
        /// <param name="script"></param>
        public void RegisterScript(string script)
        {
            Contract.Requires<ArgumentNullException>(script != null);
            Contract.Requires<InvalidOperationException>(Document != null);

            (scripts ?? (scripts = new LinkedList<string>())).AddLast(script);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public void Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            // release any existing document
            Release();

            // load new document
            document = new Lazy<Document>(() => Document.Load(uri, catalog, exports));
            OnDocumentLoaded(DocumentEventArgs.Empty);
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
        /// Loads the docuemnt from the given <see cref="TextReader"/> into the server.
        /// </summary>
        /// <param name="reader"></param>
        public void Load(TextReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            // release any existing document
            Release();

            // load new document
            document = new Lazy<NXKit.Document>(() => Document.Load(reader, catalog, exports));
            OnDocumentLoaded(DocumentEventArgs.Empty);
        }

        /// <summary>
        /// Loads the docuemnt from the given <see cref="XmlReader"/> into the server.
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            // release any existing document
            Release();

            // load new document
            document = new Lazy<NXKit.Document>(() => Document.Load(reader, catalog, exports));
            OnDocumentLoaded(DocumentEventArgs.Empty);
        }

        /// <summary>
        /// Loads the document from the given saved state string.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        void LoadSave(string save)
        {
            Contract.Requires<ArgumentNullException>(save != null);

            using (var stm = new MemoryStream(Encoding.ASCII.GetBytes(save)))
            using (var b64 = new CryptoStream(stm, new FromBase64Transform(), CryptoStreamMode.Read))
            using (var cmp = new DeflateStream(b64, CompressionMode.Decompress))
            using (var rdr = XmlDictionaryReader.CreateBinaryReader(cmp, new XmlDictionaryReaderQuotas()))
            {
                Load(rdr);
            }
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
        string GetSaveString()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            using (var stream = new MemoryStream())
            using (var encode = new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write))
            using (var deflate = new DeflateStream(encode, CompressionMode.Compress))
            using (var writer = XmlDictionaryWriter.CreateBinaryWriter(deflate))
            {
                document.Value.Save(writer);

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
        JToken CreateNodeJObject()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            // serialize document state to data field
            using (var wrt = new JTokenWriter())
            {
                RemoteHelper.GetJson(wrt, document.Value.Root);
                return wrt.Token;
            }
        }

        /// <summary>
        /// Gets the client-side message data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken GetMessagesObject()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            return JArray.FromObject(document.Value.Container.GetExportedValue<TraceSink>().Messages);
        }

        /// <summary>
        /// Gets the client-side script data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken GetScriptsObject()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            return new JArray(scripts);
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken GetDataObject()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            return new JObject(
                new JProperty("Node",
                    CreateNodeJObject()),
                new JProperty("Messages",
                    GetMessagesObject()),
                new JProperty("Scripts",
                    GetScriptsObject()));
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="String"/>.
        /// </summary>
        /// <returns></returns>
        string GetDataString()
        {
            Contract.Requires<InvalidOperationException>(document != null);
            Contract.Requires<InvalidOperationException>(document.Value != null);

            return JsonConvert.SerializeObject(GetDataObject());
        }

        /// <summary>
        /// Returns a saved version of the currently loaded document.
        /// </summary>
        /// <returns></returns>
        public JObject Save()
        {
            try
            {
                // notify any subscribers to conduct final operations
                if (Document != null)
                    OnDocumentUnloading(DocumentEventArgs.Empty);

                // extract data from document
                var data = Document != null ? GetDataObject() : null;
                var save = Document != null ? GetSaveString() : null;
                var hash = save != null ? GetMD5HashText(save) : null;

                // cache save data
                if (save != null &&
                    hash != null)
                    cache.Add(hash, save);

                // respond with object containing new save and JSON tree
                return new JObject(
                    new JProperty[] {
                        new JProperty("Code", code),
                        save != null ? new JProperty("Save", save) : null,
                        hash != null ? new JProperty("Hash", hash) : null,
                        data != null ? new JProperty("Data", data) : null, }
                        .Where(i => i != null));
            }
            finally
            {
                code = ViewResponseCode.Unknown;
            }
        }

        /// <summary>
        /// Loads the given <see cref="Document"/> from the given hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool LoadFromHash(string hash)
        {
            if (hash != null)
            {
                var save = (string)cache.Get(hash);
                if (save != null)
                    return LoadFromSave(save);
            }

            return false;
        }

        /// <summary>
        /// Loads the <see cref="Document"/> from the given save data.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        bool LoadFromSave(string save)
        {
            if (save != null)
            {
                LoadSave(save);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads and executes the object.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ViewResponseCode LoadAndExecute(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            // load save data
            var save = (string)args["Save"];
            if (save != null)
                if (LoadFromSave(save))
                    return Execute(args, null);

            // load hash data
            var hash = (string)args["Hash"];
            if (hash != null)
                if (LoadFromHash(hash))
                    return Execute(args, hash);

            return ViewResponseCode.NeedSave;
        }

        /// <summary>
        /// Loads a saved version of the document.
        /// </summary>
        /// <param name="args"></param>
        public ViewResponseCode Load(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            return LoadAndExecute(args);
        }

        /// <summary>
        /// Executes the incoming argument set.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ViewResponseCode Execute(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            // loads and executes the document
            return LoadAndExecute(args);
        }

        /// <summary>
        /// Executes the incoming argument structure.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="saveHash"></param>
        /// <returns></returns>
        ViewResponseCode Execute(JObject args, string saveHash)
        {
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Requires<InvalidOperationException>(document != null);

            // execute any passed commands
            return code = ExecuteCommands(args);
        }

        /// <summary>
        /// Executes the commands packed in the arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ViewResponseCode ExecuteCommands(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Requires<InvalidOperationException>(document != null);

            var commands = (JArray)args["Commands"];
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    // dispatch action
                    switch ((string)command["Action"])
                    {
                        case "Update":
                            JsonInvokeMethod(
                                typeof(ViewServer).GetMethod("ClientUpdate", BindingFlags.NonPublic | BindingFlags.Instance),
                                (JObject)command["Args"]);
                            break;
                        case "Invoke":
                            JsonInvokeMethod(
                                typeof(ViewServer).GetMethod("ClientInvoke", BindingFlags.NonPublic | BindingFlags.Instance),
                                (JObject)command["Args"]);
                            break;
                    }
                }
            }

            return ViewResponseCode.Good;
        }

        /// <summary>
        /// Invokes the given method with the specified parameter values.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        void JsonInvokeMethod(MethodInfo method, JObject args)
        {
            Contract.Requires<ArgumentNullException>(method != null);
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Requires<InvalidOperationException>(document != null);

            // assembly invocation parameter list
            var count = 0;
            var parameters = method.GetParameters();
            var invoke = new object[parameters.Length];
            for (int i = 0; i < invoke.Length; i++)
            {
                // submitted JSON parameter value
                var j = args.Properties()
                    .FirstOrDefault(k => string.Equals(parameters[i].Name, k.Name, StringComparison.InvariantCultureIgnoreCase));
                if (j == null)
                    break;

                // convert JObject to appropriate type
                var t = parameters[i].ParameterType;
                var o = j != null && j.Value != null ? j.Value.ToObject(t) : null;

                // successful conversion
                invoke[i] = o;
                count = i + 1;
            }

            // unsuccessful parameter count
            if (count != parameters.Length)
                throw new MissingMethodException();

            method.Invoke(this, invoke);
        }

        /// <summary>
        /// Updates the given property.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="interface"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void ClientUpdate(int nodeId, string @interface, string property, JValue value)
        {
            Contract.Requires<ArgumentOutOfRangeException>(nodeId > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(@interface));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(property));
            Contract.Requires<InvalidOperationException>(document != null);

            var node = (XNode)document.Value.Xml.ResolveObjectId(nodeId);
            if (node == null)
                return;

            RemoteHelper.Update(node, @interface, property, value);
        }

        /// <summary>
        /// Invokes the given method.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="interface"></param>
        /// <param name="method"></param>
        /// <param name="params"></param>
        void ClientInvoke(int nodeId, string @interface, string method, JObject @params)
        {
            Contract.Requires<ArgumentOutOfRangeException>(nodeId > 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(@interface));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(method));
            Contract.Requires<InvalidOperationException>(document != null);

            var node = (XNode)document.Value.Xml.ResolveObjectId(nodeId);
            if (node == null)
                return;

            RemoteHelper.Invoke(node, @interface, method, @params);
        }

        /// <summary>
        /// Disposes of the server.
        /// </summary>
        public void Dispose()
        {
            if (document != null)
            {
                // only dispose if document loaded
                if (document.IsValueCreated)
                    document.Value.Dispose();

                document = null;
            }
        }

    }

}
