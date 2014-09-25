using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Web.UI;

using Newtonsoft.Json;

using NXKit.Server;

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
        ViewMessage message;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public View()
        {
            this.server = new ViewServer();
            this.server.DocumentLoaded += (s, a) => OnDocumentLoaded(a);
            this.server.DocumentUnloading += (s, a) => OnDocumentUnloading(a);
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
        /// Gets or sets the additional set of exports to introduce to newly generated documents.
        /// </summary>
        public ExportProvider Exports
        {
            get { return server.Exports; }
            set { server.Exports = value; }
        }

        /// <summary>
        /// Gets or sets the additional set of parts to introduce to newly generated documents.
        /// </summary>
        public ComposablePartCatalog Catalog
        {
            get { return server.Catalog; }
            set { server.Catalog = value; }
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
            if (DocumentLoaded != null)
                DocumentLoaded(this, args);
        }

        /// <summary>
        /// Raised when the <see cref="Document"/> is unloading.
        /// </summary>
        public event DocumentUnloadingEventHandler DocumentUnloading;

        /// <summary>
        /// Raises the DocumentUnloading event.
        /// </summary>
        /// <param name="args"></param>
        void OnDocumentUnloading(DocumentEventArgs args)
        {
            if (DocumentUnloading != null)
                DocumentUnloading(this, args);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            message = server.Load(uri);
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            message = server.Load(uri);
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
            base.OnPreRender(args);

            if (message != null)
            {
                ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
                Page.ClientScript.RegisterOnSubmitStatement(typeof(View), GetHashCode().ToString(), @"$find('" + ClientID + @"')._onsubmit();");
            }
        }

        /// <summary>
        /// Loads view state information.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            var o = (object[])savedState;
            if ((string)o[0] != null)
                message = server.Load(JsonConvert.DeserializeObject<ViewMessage>(((string)o[0])));
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
                !Visible && message != null ? JsonConvert.SerializeObject(message) : null,
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
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "body");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            writer.WriteLine();

            if (message != null)
            {
                ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

                // serialize visual state to data field
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "data");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, JsonConvert.SerializeObject(message));
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
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var d = new ScriptControlDescriptor("_NXKit.Web.UI.View", ClientID);
            d.AddProperty("sendFunc", Page.ClientScript.GetCallbackEventReference(this, "args", "cb", "self") + ";");
            yield return d;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference() { Name = "jquery" };
            yield return new ScriptReference() { Name = "knockout" };
            yield return new ScriptReference() { Name = "nxkit" };
            yield return new ScriptReference() { Name = "nxkit-ui" };
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

            var text = postCollection[postDataKey];
            if (!string.IsNullOrWhiteSpace(text))
                message = server.Load(JsonConvert.DeserializeObject<ViewMessage>(text));

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
            message = server.Load(JsonConvert.DeserializeObject<ViewMessage>(eventArgument));
        }

        /// <summary>
        /// Gets the result of a client callback.
        /// </summary>
        /// <returns></returns>
        string ICallbackEventHandler.GetCallbackResult()
        {
            return JsonConvert.SerializeObject(message);
        }

    }

}
