using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NXKit.Web.Serialization;
using NXKit.Xml;

namespace NXKit.Web.UI
{

    [ToolboxData("<{0}:View runat=\"server\"></{0}:View>")]
    public class View :
        Control,
        INamingContainer,
        IPostBackDataHandler,
        ICallbackEventHandler,
        IScriptControl
    {

        /// <summary>
        /// Gets the MD5 hash of the given data string in text format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static string GetMD5HashText(string data)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
            var text = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                text.Append(hash[i].ToString("x2"));
            return text.ToString();
        }


        string cssClass;
        string validationGroup;
        ComposablePartCatalog catalog;
        ExportProvider exports;
        NXDocumentHost host;
        LinkedList<string> scripts;
        Func<string> responseFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public View()
        {

        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        [ThemeableAttribute(true)]
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        /// <summary>
        /// Gets or sets the group of controls for which the <see cref="View"/> control causes validation when it posts back to the server.
        /// </summary>
        [ThemeableAttribute(false)]
        public string ValidationGroup
        {
            get { return validationGroup; }
            set { validationGroup = value; }
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
        /// Gets a reference to the <see cref="Host"/>.
        /// </summary>
        public NXDocumentHost Host
        {
            get { return host; }
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
        NXDocumentHost LoadDocumentHost(string save)
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
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            host = NXDocumentHost.Load(uri, catalog, exports);
            OnHostLoaded(HostEventArgs.Empty);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            Open(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Gets the client-side save state as a string.
        /// </summary>
        /// <returns></returns>
        string CreateSaveString()
        {
            if (host == null)
                return null;

            using (var stm = new MemoryStream())
            using (var b64 = new CryptoStream(stm, new ToBase64Transform(), CryptoStreamMode.Write))
            using (var cmp = new DeflateStream(b64, CompressionMode.Compress))
            using (var xml = XmlDictionaryWriter.CreateBinaryWriter(cmp))
            {
                host.Save(xml);

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
                RemoteHelper.GetJson(wrt, host.Root);
                return wrt.Token;
            }
        }

        /// <summary>
        /// Gets the client-side message data as a <see cref="JToken"/>.
        /// </summary>
        /// <returns></returns>
        JToken CreateMessagesJObject()
        {
            return JArray.FromObject(host.Container.GetExportedValue<TraceSink>().Messages);
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

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(args);
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPreRender(EventArgs args)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
            base.OnPreRender(args);

            // write all available knockout templates
            if (Host != null)
                foreach (var provider in Host.Container.GetExportedValues<IHtmlTemplateProvider>())
                    foreach (var template in provider.GetTemplates())
                        if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(View), template.Name))
                            using (var rdr = new StreamReader(template.Open()))
                                Page.ClientScript.RegisterClientScriptBlock(typeof(View), template.Name, rdr.ReadToEnd(), false);
        }

        /// <summary>
        /// Loads view state information.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            var o = (object[])savedState;
            host = (string)o[0] != null ? LoadDocumentHost((string)o[0]) : null;
            cssClass = (string)o[1];
            validationGroup = (string)o[2];
        }

        /// <summary>
        /// Saves view state information.
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            return new object[] 
            {
                !Visible ? CreateSaveString() : null,
                cssClass,
                validationGroup,
            };
        }

        /// <summary>
        /// Renders the server control to the client.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_body");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            writer.WriteLine();

            // serialize visual state to data field
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.WriteLine();

            if (host != null)
            {
                // allow final shut down
                OnHostUnloading(HostEventArgs.Empty);

                // generate host data
                var data = CreateDataString();
                var save = CreateSaveString();
                var hash = GetMD5HashText(save);

                // dispose of host
                host.Dispose();
                host = null;

                // cache save data
                Context.Cache.Insert(hash, save, null, DateTime.UtcNow.AddMinutes(5), Cache.NoSlidingExpiration);

                // serialize visual state to data field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_data");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_data");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, data);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();

                // serialize document state to save field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, save);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();

                // serialize document state to save field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_hash");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_hash");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, hash);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();
            }

            writer.RenderEndTag();
            writer.WriteLine();
        }

        /// <summary>
        /// Raises the Unload event.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUnload(EventArgs args)
        {
            base.OnUnload(args);

            if (host != null)
            {
                OnHostUnloading(HostEventArgs.Empty);
                host.Dispose();
                host = null;
            }
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var d = new ScriptControlDescriptor("_NXKit.Web.UI.View", ClientID);
            d.AddElementProperty("body", ClientID + "_body");
            d.AddElementProperty("save", ClientID + "_save");
            d.AddElementProperty("hash", ClientID + "_hash");
            d.AddElementProperty("data", ClientID + "_data");
            d.AddProperty("push", Page.ClientScript.GetCallbackEventReference(this, "args", "cb", "self") + ";");
            yield return d;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.XForms.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.XForms.Layout.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.View.js", typeof(View).Assembly.FullName);
        }

        /// <summary>
        /// Loads the given <see cref="NXDocumentHost"/> from the given hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool LoadFromHash(string hash)
        {
            if (hash != null)
            {
                var save = (string)Context.Cache.Get(hash);
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
        bool LoadFromSave(string save)
        {
            if (save != null)
            {
                host = LoadDocumentHost(save);
                OnHostLoaded(HostEventArgs.Empty);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads any view state from custom fields.
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (Page.IsCallback)
                return false;

            // load saved data
            var save = postCollection[postDataKey + "_save"];
            if (save != null)
                if (LoadFromSave(save))
                    return true;

            var hash = postCollection[postDataKey + "_hash"];
            if (hash != null)
                if (LoadFromHash(hash))
                    return true;

            return false;
        }

        /// <summary>
        /// Raised when the post data has changed.
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {

        }

        /// <summary>
        /// Raised when a client callback is invoked.
        /// </summary>
        /// <param name="eventArgument"></param>
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            responseFunc = Execute(JObject.Parse(eventArgument));
        }

        /// <summary>
        /// Loads the <see cref="NXDocumentHost"/> from the given argument data.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Func<string> Execute(JObject args)
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
                OnHostUnloading(HostEventArgs.Empty);

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
                        Context.Cache.Insert(hash, save, null, DateTime.UtcNow.AddMinutes(5), Cache.NoSlidingExpiration);

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
                    host.Dispose();
                    host = null;
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
                            JsonInvokeMethod(typeof(View).GetMethod("ClientUpdate", BindingFlags.NonPublic | BindingFlags.Instance), (JObject)command["Args"]);
                            break;
                        case "Invoke":
                            JsonInvokeMethod(typeof(View).GetMethod("ClientInvoke", BindingFlags.NonPublic | BindingFlags.Instance), (JObject)command["Args"]);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the result of a client callback.
        /// </summary>
        /// <returns></returns>
        string ICallbackEventHandler.GetCallbackResult()
        {
            return responseFunc != null ? responseFunc() : null;
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

            var node = (XNode)host.Xml.ResolveObjectId(nodeId);
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

            var node = (XNode)host.Xml.ResolveObjectId(nodeId);
            if (node == null)
                return;

            RemoteHelper.Invoke(node, @interface, method, @params);
        }

    }

}
