using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using NXKit.Diagnostics;
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

        enum LogLevel
        {

            Verbose,
            Information,
            Warning,
            Error,

        }

        /// <summary>
        /// Log item to be output to the client.
        /// </summary>
        [Serializable]
        class Log
        {

            DateTime timestamp;
            LogLevel level;
            string message;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public Log()
            {
                this.timestamp = DateTime.UtcNow;
                this.level = LogLevel.Information;
                this.message = null;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="level"></param>
            /// <param name="message"></param>
            public Log(LogLevel level, string message)
            {
                this.timestamp = DateTime.UtcNow;
                this.level = level;
                this.message = message;
            }

            public DateTime Timestamp
            {
                get { return timestamp; }
                set { timestamp = value; }
            }

            [JsonConverter(typeof(StringEnumConverter))]
            public LogLevel Level
            {
                get { return level; }
                set { level = value; }
            }

            public string Message
            {
                get { return message; }
                set { message = value; }
            }

        }

        /// <summary>
        /// Captures trace messages from the <see cref="NXDocumentHost"/> to be output to the client.
        /// </summary>
        class LogSink :
            ITraceSink
        {

            readonly LinkedList<Log> logs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="logs"></param>
            public LogSink(LinkedList<Log> logs)
            {
                Contract.Requires<ArgumentNullException>(logs != null);

                this.logs = logs;
            }

            public void Debug(object data)
            {
                logs.AddLast(new Log(LogLevel.Verbose, data.ToString()));
            }

            public void Debug(string message)
            {
                logs.AddLast(new Log(LogLevel.Verbose, message));
            }

            public void Debug(string format, params object[] args)
            {
                logs.AddLast(new Log(LogLevel.Verbose, string.Format(format, args)));
            }

            public void Information(object data)
            {
                logs.AddLast(new Log(LogLevel.Information, data.ToString()));
            }

            public void Information(string message)
            {
                logs.AddLast(new Log(LogLevel.Information, message));
            }

            public void Information(string format, params object[] args)
            {
                logs.AddLast(new Log(LogLevel.Information, string.Format(format, args)));
            }

            public void Warning(object data)
            {
                logs.AddLast(new Log(LogLevel.Warning, data.ToString()));
            }

            public void Warning(string message)
            {
                logs.AddLast(new Log(LogLevel.Warning, message));
            }

            public void Warning(string format, params object[] args)
            {
                logs.AddLast(new Log(LogLevel.Warning, string.Format(format, args)));
            }

        }

        string cssClass;
        string validationGroup;
        ComposablePartCatalog catalog;
        ExportProvider exports;
        CompositionContainer container;
        NXDocumentHost document;
        LinkedList<Log> logs;
        LinkedList<Log> logs_;

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
        /// Gets a reference to the <see cref="Document"/>.
        /// </summary>
        public NXDocumentHost Document
        {
            get { return document; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ComposablePartCatalog"/> used to resolve exports.
        /// </summary>
        public ComposablePartCatalog Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }

        /// <summary>
        /// Gets or sets a <see cref="ExportProvider"/> used to resolve exports.
        /// </summary>
        public ExportProvider Exports
        {
            get { return exports; }
            set { exports = value; }
        }

        /// <summary>
        /// Loads the document host from whatever saved state is available.
        /// </summary>
        /// <returns></returns>
        NXDocumentHost LoadDocumentHost(string save)
        {
            Contract.Requires<ArgumentNullException>(save != null);

            // extend provided container
            container = new CompositionContainer(exports)
                .WithExport<ITraceSink>(new LogSink(logs ?? (logs = new LinkedList<Log>())));

            // load document
            return NXDocumentHost.Load(new StringReader(save), catalog, container);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            // extend provided container
            container = new CompositionContainer(exports)
                .WithExport<ITraceSink>(new LogSink(logs ?? (logs = new LinkedList<Log>())));

            document = NXDocumentHost.Load(uri, catalog, container);
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
            using (var wrt = new StringWriter())
            {
                document.Save(wrt);
                return wrt.ToString();
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JObject"/>
        /// </summary>
        /// <returns></returns>
        JToken CreateDataJObject()
        {
            // serialize document state to data field
            using (var wrt = new JTokenWriter())
            {
                RemoteJson.GetJson(wrt, document.Root);
                return wrt.Token;
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        string CreateDataString()
        {
            // serialize document state to data field
            using (var str = new StringWriter())
            using (var wrt = new JsonTextWriter(str))
            {
                CreateDataJObject().WriteTo(wrt);
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the client-side logs data as a <see cref="JObject"/>.
        /// </summary>
        /// <returns></returns>
        JToken CreateLogsJObject()
        {
            return logs_ != null ? JArray.FromObject(logs_) : new JArray();
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        string CreateLogsString()
        {
            // serialize document state to data field
            using (var str = new StringWriter())
            using (var wrt = new JsonTextWriter(str))
            {
                CreateLogsJObject().WriteTo(wrt);
                return str.ToString();
            }
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
            if (Document != null)
                foreach (var provider in Document.Container.GetExportedValues<IHtmlTemplateProvider>())
                    foreach (var template in provider.GetTemplates())
                        if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(View), template.Name))
                            using (var rdr = new StreamReader(template.Open()))
                                Page.ClientScript.RegisterClientScriptBlock(typeof(View), template.Name, rdr.ReadToEnd(), false);

            // logs_ to be sent
            logs_ = logs;
            logs = null;
        }

        /// <summary>
        /// Loads view state information.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            var o = (object[])savedState;
            document = (string)o[0] != null ? LoadDocumentHost((string)o[0]) : null;
            cssClass = (string)o[1];
            validationGroup = (string)o[2];
            logs = (LinkedList<Log>)o[3];
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
                logs,
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

            if (document != null)
            {
                // serialize visual state to data field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_data");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_data");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CreateDataString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();

                // serialize document state to save field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CreateSaveString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();
            }

            writer.RenderEndTag();
            writer.WriteLine();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var d = new ScriptControlDescriptor("_NXKit.Web.UI.View", ClientID);
            d.AddElementProperty("body", ClientID + "_body");
            d.AddElementProperty("data", ClientID + "_data");
            d.AddElementProperty("save", ClientID + "_save");
            d.AddProperty("logs", CreateLogsString());
            d.AddProperty("push", Page.ClientScript.GetCallbackEventReference(this, "args", "cb", "self"));
            yield return d;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.XForms.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.TypeScript.NXKit.XForms.Layout.js", typeof(View).Assembly.FullName);
            yield return new ScriptReference("NXKit.Web.UI.View.js", typeof(View).Assembly.FullName);
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (Page.IsCallback)
                return false;

            // load saved data
            var save = postCollection[postDataKey + "_save"];
            if (save != null)
                document = LoadDocumentHost(save);

            return true;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            // dump logs
            logs_ = logs;
            logs = null;

            return JsonConvert.SerializeObject(new
            {
                Save = CreateSaveString(),
                Data = CreateDataJObject(),
                Logs = CreateLogsJObject(),
            });
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            var args = JObject.Parse(eventArgument);

            var save = (string)args["Save"];
            if (save != null)
                document = LoadDocumentHost(save);

            // dispatch action
            switch ((string)args["Action"])
            {
                case "Push":
                    ClientPush((JToken)args["Args"]);
                    break;
            }
        }

        /// <summary>
        /// Client has sent us a "Push" request, consisting of a single argument "Nodes" which contains an array of node data.
        /// </summary>
        /// <param name="data"></param>
        void ClientPush(JToken args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            var nodes = args["Nodes"] as JArray;
            if (nodes == null)
                throw new InvalidOperationException("Push requires JSON array of node data.");

            foreach (JObject node in nodes)
                ClientPushNode(node);

            document.Invoke();
        }

        /// <summary>
        /// Client 
        /// </summary>
        /// <param name="data"></param>
        void ClientPushNode(JObject data)
        {
            Contract.Requires<ArgumentNullException>(data != null);

            var id = (int)data["Id"];
            if (id < 0)
                throw new InvalidOperationException("Client Push sent invalid Node ID.");

            var node = document.Root.DescendantsAndSelf()
                .FirstOrDefault(i => i.GetObjectId() == id);
            if (node == null)
                throw new InvalidOperationException("Client Push sent unknown Node ID.");

            ApplyToNode(node, data);
        }

        void ApplyToNode(XElement node, JObject data)
        {
            Contract.Requires<ArgumentNullException>(data != null);

            RemoteJson.SetJson(data.CreateReader(), node);
        }

    }

}
