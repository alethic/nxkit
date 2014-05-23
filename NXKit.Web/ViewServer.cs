using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
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
    /// Hosts a <see cref="NXDocumentHost"/> instance and provides interaction services for the client Web UI.
    /// </summary>
    public class ViewServer :
        IDisposable
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
        public event DocumentLoadedEventHandler HostLoaded;

        /// <summary>
        /// Raises the HostLoaded event.
        /// </summary>
        /// <param name="args"></param>
        void OnHostLoaded(DocumentEventArgs args)
        {
            if (HostLoaded != null)
                HostLoaded(this, args);
        }

        /// <summary>
        /// Raised when the <see cref="NXDocumentHost"/> is unloading.
        /// </summary>
        public event DocumentLoadedEventHandler HostUnloading;

        /// <summary>
        /// Raises the HostUnloading event.
        /// </summary>
        /// <param name="args"></param>
        void OnHostUnloading(DocumentEventArgs args)
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
        /// Gets the MD5 hash of the given data string in text format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetMD5HashText(string data)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
            var text = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                text.Append(hash[i].ToString("x2"));
            return text.ToString();
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the server.
        /// </summary>
        /// <param name="uri"></param>
        public void Load(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            document = NXDocumentHost.Load(uri, catalog, exports);
            OnHostLoaded(DocumentEventArgs.Empty);
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

            document = NXDocumentHost.Load(reader, catalog, exports);
            OnHostLoaded(DocumentEventArgs.Empty);
        }

        /// <summary>
        /// Loads the docuemnt from the given <see cref="XmlReader"/> into the server.
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            document = NXDocumentHost.Load(reader, catalog, exports);
            OnHostLoaded(DocumentEventArgs.Empty);
        }

        /// <summary>
        /// Loads the document host from the given saved state.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        public void LoadSave(string save)
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
        /// Gets the client-side save state as a string.
        /// </summary>
        /// <returns></returns>
        public string CreateSaveString()
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
        public string CreateDataString()
        {
            return JsonConvert.SerializeObject(CreateDataJObject());
        }

        /// <summary>
        /// Loads the given <see cref="NXDocumentHost"/> from the given hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool LoadFromHash(string hash)
        {
            if (hash != null)
            {
                var save = (string)MemoryCache.Default.Get(hash);
                if (save != null)
                    return LoadFromSave(save);
            }

            return false;
        }

        /// <summary>
        /// Loads the <see cref="NXDocumentHost"/> from the given save data.
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        public bool LoadFromSave(string save)
        {
            if (save != null)
            {
                LoadSave(save);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the <see cref="NXDocumentHost"/> from the given argument data.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Func<string> Execute(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Ensures(Contract.Result<Func<string>>() != null);

            var save = (string)args["Save"];
            if (save != null)
                if (LoadFromSave(save))
                    return Execute(args, GetMD5HashText(save));

            var hash = (string)args["Hash"];
            if (hash != null)
                if (LoadFromHash(hash))
                    return Execute(args, hash);

            // could not retrieve saved document
            // respond by asking for full save data
            return () => JsonConvert.SerializeObject(new
            {
                Code = ViewResponseCode.NeedSave,
            });
        }

        /// <summary>
        /// Executes the incoming argument structure.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="hash"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Func<string> Execute(JObject args, string saveHash)
        {
            // execute any passed commands
            ExecuteCommands(args);

            // return function to generate result
            return () =>
            {
                // allow final shut down
                OnHostUnloading(DocumentEventArgs.Empty);

                try
                {
                    // extract data from document
                    var data = CreateDataJObject();
                    var save = CreateSaveString();
                    var hash = GetMD5HashText(save);

                    // document has changed
                    if (hash != saveHash)
                    {
                        // cache save data
                        MemoryCache.Default.Add(hash, save, DateTime.UtcNow.AddMinutes(5));

                        // respond with object containing new save and JSON tree
                        return JsonConvert.SerializeObject(new
                        {
                            Code = ViewResponseCode.Good,
                            Save = save,
                            Hash = hash,
                            Data = data,
                        });
                    }
                    else
                    {
                        // respond with object without new save data
                        return JsonConvert.SerializeObject(new
                        {
                            Code = ViewResponseCode.Good,
                            Hash = hash,
                            Data = data,
                        });
                    }
                }
                finally
                {
                    // dispose of the host
                    document.Dispose();
                    document = null;
                }
            };
        }

        /// <summary>
        /// Executes the commands packed in the arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        void ExecuteCommands(JObject args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            var commands = (JArray)args["Commands"];
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    // dispatch action
                    switch ((string)command["Action"])
                    {
                        case "Update":
                            JsonInvokeMethod(typeof(ViewServer).GetMethod("ClientUpdate", BindingFlags.NonPublic | BindingFlags.Instance), (JObject)command["Args"]);
                            break;
                        case "Invoke":
                            JsonInvokeMethod(typeof(ViewServer).GetMethod("ClientInvoke", BindingFlags.NonPublic | BindingFlags.Instance), (JObject)command["Args"]);
                            break;
                    }
                }
            }
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

            var node = (XNode)document.Xml.ResolveObjectId(nodeId);
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

            var node = (XNode)document.Xml.ResolveObjectId(nodeId);
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
                document.Dispose();
                document = null;
            }
        }

    }

}
