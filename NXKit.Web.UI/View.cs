using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Web.IO;

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
        /// Private resolver implementation to dispatch to events.
        /// </summary>
        class ResourceResolver :
            IResolver
        {

            readonly View control;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="control"></param>
            internal ResourceResolver(View control)
            {
                this.control = control;
            }

            public Stream Get(Uri uri)
            {
                return Resolve(ResourceActionMethod.Get, uri, null);
            }

            public Stream Put(Uri uri, Stream body)
            {
                return Resolve(ResourceActionMethod.Put, uri, body);
            }

            Stream Resolve(ResourceActionMethod method, Uri uri, Stream body)
            {
                var args = new ResourceActionEventArgs(method, uri, body);
                control.OnResourceAction(args);
                return args.Result;
            }

        }

        string cssClass;
        string validationGroup;
        NXDocument document;

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
        public NXDocument Document
        {
            get { return document; }
        }

        /// <summary>
        /// Raised when the forms processor attempts to perform an action on a resource.
        /// </summary>
        public event EventHandler<ResourceActionEventArgs> ResourceAction;

        /// <summary>
        /// Raises the ResolveResource event.
        /// </summary>
        /// <param name="args"></param>
        void OnResourceAction(ResourceActionEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            if (ResourceAction != null)
                ResourceAction(this, args);
        }

        /// <summary>
        /// Attempts to resolve a resource's local uri to a URI suitable for offering a link to the user.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public string ResolveResourceClientUrl(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            var args = new ResourceActionEventArgs(ResourceActionMethod.ResolveClientUrl, uri);
            OnResourceAction(args);
            return args.ReferenceUri;
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Configure(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            document = new NXDocument(
                CompositionUtil.CreateContainer()
                    .WithExport<IResolver>(new ResourceResolver(this)),
                uri);
            document.Invoke();
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Configure(string uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            Configure(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Gets the client-side save state as a string.
        /// </summary>
        /// <returns></returns>
        string GetSaveString()
        {
            // serialize document state to save field
            using (var stm = new MemoryStream())
            using (var zip = new GZipStream(stm, CompressionMode.Compress))
            {
                new BinaryFormatter().Serialize(zip, document.Save());
                zip.Close();
                return Convert.ToBase64String(stm.ToArray());
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="JObject"/>
        /// </summary>
        /// <returns></returns>
        JObject GetDataJObject()
        {
            // serialize document state to data field
            using (var str = new JTokenWriter())
            using (var wrt = new JsonNodeWriter(str))
            {
                wrt.Write(document.Root);
                wrt.Close();
                return (JObject)str.Token;
            }
        }

        /// <summary>
        /// Gets the client-side data as a <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        string GetDataString()
        {
            // serialize document state to data field
            using (var str = new StringWriter())
            using (var wrt = new JsonNodeWriter(str))
            {
                wrt.Write(document.Root);
                wrt.Close();
                return str.ToString();
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);

            // write all available knockout templates
            if (Document != null)
                foreach (var provider in Document.Container.GetExportedValues<IHtmlTemplateProvider>())
                    foreach (var template in provider.GetTemplates())
                        if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(View), template.Name))
                            using (var rdr = new StreamReader(template.Open()))
                                Page.ClientScript.RegisterClientScriptBlock(typeof(View), template.Name, rdr.ReadToEnd(), false);
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
            writer.WriteLine(@"<!-- ko with: new NXKit.Web.DefaultLayoutManager($context) -->");
            writer.WriteLine(@"<!-- ko nxkit_template: $parent -->");
            writer.WriteLine(@"<!-- /ko -->");
            writer.WriteLine(@"<!-- /ko -->");
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
                writer.AddAttribute(HtmlTextWriterAttribute.Value, GetDataString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();

                // serialize document state to save field
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_save");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, GetSaveString());
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

        /// <summary>
        /// Loads the <see cref="NXDocument"/> from the given saved state.
        /// </summary>
        /// <param name="save"></param>
        void LoadDocumentFromSave(string save)
        {
            using (var stm = new MemoryStream(Convert.FromBase64String(save)))
            using (var zip = new GZipStream(stm, CompressionMode.Decompress))
            {
                // deserialize value into state
                var state = (NXDocumentState)new BinaryFormatter().Deserialize(zip);
                if (state == null)
                    throw new NullReferenceException();

                document = new NXDocument(
                    CompositionUtil.CreateContainer()
                        .WithExport<IResolver>(new ResourceResolver(this)),
                    state);
                document.Invoke();
            }
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            var save = postCollection[postDataKey + "_save"];
            if (save != null)
                LoadDocumentFromSave(save);

            return true;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return JsonConvert.SerializeObject(new
            {
                Save = GetSaveString(),
                Data = GetDataJObject(),
            });
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            dynamic args = JObject.Parse(eventArgument);

            // attempt to load our document instance
            var save = (string)args.Save;
            if (save != null)
                LoadDocumentFromSave(save);

            // dispatch action
            switch ((string)args.Action)
            {
                case "Push":
                    ClientPush((JObject)args.Args.Data);
                    break;
            }
        }

        /// <summary>
        /// Client has sent us a complete object tree.
        /// </summary>
        /// <param name="data"></param>
        void ClientPush(JObject data)
        {
            VisitAndPush(data, document.Root);
            document.Invoke();
        }

        void VisitAndPush(JObject s, NXNode d)
        {
            if (s.Value<string>("Type") != d.GetType().FullName)
                throw new Exception();

            var sProperties = s.Value<JObject>("Properties")
                .Properties()
                .OrderBy(i => i.Name)
                .ToArray();

            var dProperties = TypeDescriptor.GetProperties(d)
                .Cast<PropertyDescriptor>()
                .Where(i => i.Attributes.OfType<InteractiveAttribute>().Any())
                .OrderBy(i => i.Name)
                .ToArray();

            // join property lists by name
            var pL = dProperties
                .Join(sProperties, i => i.Name, i => i.Name, (dP, sP) => new { sP, dP })
                .Where(i => !i.dP.IsReadOnly)
                .Where(i => ((JObject)i.sP.Value).Value<int>("Version") > 0);

            // set all properties in the destination with their matching value from the source
            foreach (var i in pL)
                i.dP.SetValue(d, ((JObject)i.sP.Value).Value<string>("Value"));

            if (d is NXElement)
            {
                var sNodes = s.Value<JArray>("Nodes")
                    .Values<JObject>()
                    .ToArray();

                var dNodes = ((NXElement)d)
                    .Nodes()
                    .ToArray();

                foreach (var i in sNodes.Zip(dNodes, (sV, dV) => new { sV, dV }))
                    VisitAndPush(i.sV, i.dV);
            }
        }

    }

}
