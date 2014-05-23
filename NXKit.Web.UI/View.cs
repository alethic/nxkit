using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.IO;
using System.Web.UI;

using Newtonsoft.Json.Linq;

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

        string cssClass;
        string validationGroup;
        ViewServer server;
        Func<string> responseFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public View()
        {
            this.server = new ViewServer();
            this.server.HostLoaded += (s, a) => OnDocumentLoaded(a);
            this.server.HostUnloading += (s, a) => OnHostUnloading(a);
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
            get { return server.Exports; }
            set { server.Exports = value; }
        }

        /// <summary>
        /// Gets or sets the additional set of parts to introduce to the document.
        /// </summary>
        public ComposablePartCatalog Catalog
        {
            get { return server.Catalog; }
            set { server.Catalog = value; }
        }

        /// <summary>
        /// Gets a reference to the <see cref="NXDocumentHost"/>.
        /// </summary>
        public NXDocumentHost Document
        {
            get { return server.Document; }
        }

        /// <summary>
        /// Raised when the <see cref="NXDocumentHost"/> is loaded.
        /// </summary>
        public event DocumentLoadedEventHandler DocumentLoaded;

        /// <summary>
        /// Raises the DocumentLoaded event.
        /// </summary>
        /// <param name="args"></param>
        void OnDocumentLoaded(DocumentEventArgs args)
        {
            if (DocumentLoaded != null)
                DocumentLoaded(this, args);
        }

        /// <summary>
        /// Raised when the <see cref="NXDocumentHost"/> is unloading.
        /// </summary>
        public event DocumentUnloadingEventHandler DocumentUnloading;

        /// <summary>
        /// Raises the DocumentUnloading event.
        /// </summary>
        /// <param name="args"></param>
        void OnHostUnloading(DocumentEventArgs args)
        {
            if (DocumentUnloading != null)
                DocumentUnloading(this, args);
        }

        /// <summary>
        /// Registers the given script snippet for execution upon completion of the current page request or client
        /// callback.
        /// </summary>
        /// <param name="script"></param>
        public void RegisterScript(string script)
        {
            server.RegisterScript(script);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            server.Load(uri);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            server.Load(uri);
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
        }

        /// <summary>
        /// Loads view state information.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            var o = (object[])savedState;
            if ((string)o[0] != null) server.LoadSave((string)o[0]);
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
                !Visible ? server.CreateSaveString() : null,
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

            if (server.Document != null)
            {
                // allow final shut down
                OnHostUnloading(DocumentEventArgs.Empty);

                // generate host data
                var data = server.CreateDataString();
                var save = server.CreateSaveString();
                var hash = server.GetMD5HashText(save);

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

            if (server != null)
            {
                server.Dispose();
                server = null;
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
                if (server.LoadFromSave(save))
                    return true;

            var hash = postCollection[postDataKey + "_hash"];
            if (hash != null)
                if (server.LoadFromHash(hash))
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
            responseFunc = server.Execute(JObject.Parse(eventArgument));
        }

        /// <summary>
        /// Gets the result of a client callback.
        /// </summary>
        /// <returns></returns>
        string ICallbackEventHandler.GetCallbackResult()
        {
            return responseFunc != null ? responseFunc() : null;
        }

    }

}
