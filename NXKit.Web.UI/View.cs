using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Util;
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

        string cssClass;
        string validationGroup;
        NXKit.NXDocumentHost document;

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
        public NXKit.NXDocumentHost Document
        {
            get { return document; }
        }

        /// <summary>
        /// Loads the specified <see cref="Uri"/> into the view.
        /// </summary>
        /// <param name="uri"></param>
        public void Open(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);

            document = NXKit.NXDocumentHost.Load(uri);
            document.Invoke();
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
        string GetSaveString()
        {
            // serialize document state to save field
            using (var stm = new MemoryStream())
            using (var zip = new GZipStream(stm, CompressionMode.Compress))
            {
                document.Save(zip);
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
            {
                RemoteJsonConvert.WriteTo(str, document.Root);
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
            using (var wrt = new JsonTextWriter(str))
            {
                RemoteJsonConvert.WriteTo(wrt, document.Root);
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
        /// Loads the <see cref="NXDocumentHost"/> from the given saved state.
        /// </summary>
        /// <param name="save"></param>
        void LoadDocumentFromSave(string save)
        {
            Contract.Requires<ArgumentNullException>(save != null);

            using (var stm = new MemoryStream(Convert.FromBase64String(save)))
            using (var zip = new GZipStream(stm, CompressionMode.Decompress))
            {
                // deserialize value into state
                var state = (NXDocumentState)new BinaryFormatter().Deserialize(zip);
                if (state == null)
                    throw new NullReferenceException();

                document = NXKit.NXDocumentHost.Load(new StringReader(save));
                document.Invoke();
            }
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (Page.IsCallback)
                return false;

            var save = postCollection[postDataKey + "_save"];
            if (save != null)
                LoadDocumentFromSave(save);

            var data = postCollection[postDataKey + "_data"];
            if (data != null)
                ClientPush(JObject.Parse(data));

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

        void VisitAndPush(JObject s, XNode d)
        {
            var items = d.Interfaces()
                .Where(i => i != null)
                .Select(i => new
                {
                    Object = i,
                    Types = TypeDescriptor.GetReflectionType(i)
                        .GetInterfaces()
                        .Concat(TypeDescriptor.GetReflectionType(i)
                            .Recurse(j => j.BaseType))
                        .Where(j => j.GetCustomAttribute<RemoteAttribute>(false) != null)
                        .ToList(),
                })
                .Where(i => i.Types.Any())
                .SelectMany(i => i.Types
                    .Select(j => new { Object = i.Object, Type = j }))
                .GroupBy(i => i.Type)
                .Select(i => new
                {
                    Type = i.Key,
                    Object = i.First().Object,
                    Properties = TypeDescriptor.GetReflectionType(i.Key)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(j => j.DeclaringType == i.Key)
                        .Where(j => j.GetCustomAttribute<RemoteAttribute>(false) != null)
                        .GroupBy(j => j.Name)
                        .Select(j => j.First())
                        .ToList(),
                    Methods = TypeDescriptor.GetReflectionType(i.Key)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(j => j.DeclaringType == i.Key)
                        .Where(j => j.GetCustomAttribute<RemoteAttribute>(false) != null)
                        .GroupBy(j => j.Name)
                        .Select(j => j.First())
                        .ToList(),
                })
                .Where(i => i.Properties.Any() || i.Methods.Any());

            foreach (var item in items)
            {
                var sourceObj = s[item.Type.FullName] as JObject;
                if (sourceObj == null)
                    continue;

                foreach (var property in item.Properties)
                {
                    var p = sourceObj.Property(property.Name);
                    if (p != null)
                    {
                        var v = p.Value;
                        var t = property.PropertyType;
                        var o = v != null ? v.ToObject(t) : null;

                        if (property.CanWrite && property.CanRead)
                            if (!object.Equals(property.GetValue(item.Object), o))
                                property.SetValue(item.Object, o);
                    }
                }

                foreach (var method in item.Methods)
                {
                    var methodObj = sourceObj['@' + method.Name] as JArray;
                    if (methodObj == null)
                        continue;

                    foreach (var invokeObj in methodObj.OfType<JObject>())
                    {
                        var count = 0;
                        var parameters = method.GetParameters();
                        var invoke = new object[parameters.Length];
                        for (int i = 0; i < invoke.Length; i++)
                        {
                            // submitted JSON parameter value
                            var j = invokeObj[parameters[i].Name];
                            if (j == null)
                                break;

                            // convert JObject to appropriate type
                            var t = parameters[i].ParameterType;
                            var o = j != null ? j.ToObject(t) : null;

                            // successful conversion
                            invoke[i] = o;
                            count = i + 1;
                        }

                        // unsuccessful parameter count, try next method
                        if (count != parameters.Length)
                            continue;

                        // successful; done with invoke object
                        method.Invoke(item.Object, invoke);
                        break;
                    }
                }
            }

            if (d is XElement)
            {
                var sNodes = s.Value<JArray>("Nodes")
                    .Values<JObject>()
                    .ToArray();

                var dNodes = ((XElement)d)
                    .Nodes()
                    .ToArray();

                foreach (var i in sNodes.Zip(dNodes, (sV, dV) => new { sV, dV }))
                    VisitAndPush(i.sV, i.dV);
            }
        }

    }

}
