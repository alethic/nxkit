using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;

using ISIS.Forms.Web.UI.Layout;
using ISIS.Forms.XForms;
using ISIS.Util;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace ISIS.Forms.Web.UI
{

    [ToolboxData("<{0}:FormView runat=\"server\"></{0}:FormView>")]
    public class FormView : Control, INamingContainer, IPostBackEventHandler, IScriptControl
    {

        /// <summary>
        /// Private resolver implementation to dispatch to events.
        /// </summary>
        private class ResourceResolver : IResourceResolver
        {

            private FormView control;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="control"></param>
            internal ResourceResolver(FormView control)
            {
                this.control = control;
            }

            public Stream Get(string href, string baseUri)
            {
                return Resolve(ResourceActionMethod.Get, href, baseUri, null);
            }

            public Stream Put(string href, string baseUri, Stream body)
            {
                return Resolve(ResourceActionMethod.Put, href, baseUri, body);
            }

            private Stream Resolve(ResourceActionMethod method, string href, string baseUri, Stream body)
            {
                var args = new ResourceActionEventArgs(method, href, baseUri, body);
                control.OnResourceAction(args);

                // return result
                return args.Result;
            }

        }

        /// <summary>
        /// Set of <see cref="VisualControlTypeDescriptor"/> instances that generate <see cref="VisualControls"/>.
        /// </summary>
        [ImportMany(typeof(VisualControlTypeDescriptor))]
        private List<VisualControlTypeDescriptor> visualControlDescriptors;

        /// <summary>
        /// Loaded modules.
        /// </summary>
        [ImportMany(typeof(FormModule), RequiredCreationPolicy=CreationPolicy.NonShared)]
        private List<FormModule> modules;

        private UpdatePanel contents;
        private CustomValidator validator;

        private IEnumerable<FormNavigation> navigations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public FormView()
            : base()
        {
            var c = new CompositionContainer(DefaultCompositionInitializer.Catalog);
            c.ComposeExportedValue(FormModule.ViewParameter, this);
            c.ComposeParts(this);
        }

        /// <summary>
        /// Resolves the <see cref="VisualControlTypeDescriptor"/> capable of handling <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public VisualControlTypeDescriptor ResolveVisualControlDescriptor(Visual visual)
        {
            return visualControlDescriptors.FirstOrDefault(i => i.CanHandleVisual(visual));
        }

        /// <summary>
        /// Creates a new <see cref="VisualControl"/> for the specified visual if available, else <c>null</c>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public VisualControl CreateVisualControl(Visual visual)
        {
            var type = ResolveVisualControlDescriptor(visual);
            if (type != null)
                return type.CreateControl(this, visual);

            return null;
        }

        /// <summary>
        /// Raised when a new <see cref="VisualControl"/> is added to the control hierarchy.
        /// </summary>
        public event VisualControlAddedEventHandler VisualControlAdded;

        /// <summary>
        /// Invokes the VisualControlAdded event.
        /// </summary>
        /// <param name="args"></param>
        internal void OnVisualControlAdded(VisualControlAddedEventArgs args)
        {
            if (VisualControlAdded != null)
                VisualControlAdded(args);
        }

        [ThemeableAttribute(false)]
        public virtual string ValidationGroup { get; set; }

        /// <summary>
        /// Gets a reference to the <see cref="FormProcessor"/>.
        /// </summary>
        public FormProcessor Form { get; private set; }

        /// <summary>
        /// Raised when the forms processor attempts to perform an action on a resource.
        /// </summary>
        public event EventHandler<ResourceActionEventArgs> ResourceAction;

        /// <summary>
        /// Raises the ResolveResource event.
        /// </summary>
        /// <param name="args"></param>
        private void OnResourceAction(ResourceActionEventArgs args)
        {
            if (ResourceAction != null)
                ResourceAction(this, args);
        }

        /// <summary>
        /// Attempts to resolve a resource's local uri to a URI suitable for offering a link to the user.
        /// </summary>
        /// <param name="href"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public string ResolveResourceClientUrl(string href, string baseUri)
        {
            var args = new ResourceActionEventArgs(ResourceActionMethod.ResolveClientUrl, href, baseUri);
            OnResourceAction(args);

            return args.ReferenceUri;
        }

        /// <summary>
        /// Gets a reference to the root navigation item, providing the possible navigation points of the form.
        /// </summary>
        public IEnumerable<FormNavigation> Navigations
        {
            get { return navigations ?? (navigations = CreateNavigations()); }
        }

        /// <summary>
        /// Creates the root navigation item.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FormNavigation> CreateNavigations()
        {
            if (Form == null)
                return null;

            return FormNavigation.CreateNavigations(null, Form.RootVisual);
        }

        /// <summary>
        /// Gets or sets the currently rendered page.
        /// </summary>
        public FormPage CurrentPage { get; set; }

        /// <summary>
        /// Invoked on the Init phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            Page.RegisterRequiresControlState(this);
        }

        /// <summary>
        /// Resets the form view to display no form.
        /// </summary>
        public void Reset()
        {
            navigations = null;
            Form = null;
            CurrentPage = null;
        }

        /// <summary>
        /// Configures the control.
        /// </summary>
        private void Configure()
        {
            // recreate on new form
            navigations = null;

            // set currently viewed page to first available, or none
            Navigate(null);
        }

        /// <summary>
        /// Configures the control.
        /// </summary>
        /// <param name="form"></param>
        public void Configure(string document)
        {
            // construct a new processor instance
            Form = new FormProcessor(document, new ResourceResolver(this));
            Form.Run();

            Configure();
        }

        /// <summary>
        /// Configures the control.
        /// </summary>
        /// <param name="form"></param>
        public void Configure(XmlDocument document)
        {
            // construct a new processor instance
            Form = new FormProcessor(document, new ResourceResolver(this));
            Form.Run();

            Configure();
        }

        /// <summary>
        /// Configures the control.
        /// </summary>
        /// <param name="form"></param>
        public void Configure(XDocument document)
        {
            // construct a new processor instance
            Form = new FormProcessor(document, new ResourceResolver(this));
            Form.Run();

            Configure();
        }

        /// <summary>
        /// Loads available control state.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadControlState(object savedState)
        {
            var state = (object[])savedState;
            base.LoadControlState(state[0]);

            // reload processor state
            var formState = (FormProcessorState)state[1];
            if (formState != null)
            {
                navigations = null;

                Form = new FormProcessor(formState, new ResourceResolver(this));
                Form.Run();

                // find current page node from state
                Navigate(Navigations
                    .SelectMany(i => i.Descendants(true))
                    .FirstOrDefault(i => i.Id == (string)state[2]));

                // set current page to first if none found
                if (CurrentPage == null)
                    Navigate(Navigations.FirstOrDefault());
            }
        }

        /// <summary>
        /// Persists required control state.
        /// </summary>
        /// <returns></returns>
        protected override object SaveControlState()
        {
            if (Form != null)
                Form.Run();

            var state = new object[4];
            state[0] = base.SaveControlState();

            // save processor configuration
            state[1] = Form != null ? Form.Save() : null;

            // save current navigation item
            state[2] = CurrentPage != null ? CurrentPage.Id : null;

            return state;
        }

        /// <summary>
        /// Invoked on the Load phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);

            // processor was just created, ensure child control hierarchy
            EnsureChildControls();
        }

        /// <summary>
        /// Sets the ID of the <see cref="VisualControl"/> properly based on it's location in the visual hierarchy.
        /// </summary>
        /// <param name="control"></param>
        internal void SetVisualControlId(VisualControl control)
        {
            var structural = control.Visual as StructuralVisual;
            if (structural != null)
            {
                // discover first parent visual control
                var parentVisualControl = control.Ascendents()
                    .OfType<VisualControl>()
                    .FirstOrDefault();
                if (parentVisualControl == null)
                    return;

                // set of visuals not rendered between control and parent visual control
                var parentVisuals = structural.Ascendants()
                    .TakeWhile(i => i != parentVisualControl.Visual);

                // separate each new visual's id to build the final ID
                var b = new StringBuilder(structural.Id);
                foreach (var parentVisual in parentVisuals)
                    b.Insert(0, "_").Insert(0, parentVisual.Id);

                control.ID = b.ToString();
            }
        }

        /// <summary>
        /// Creates the required control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            contents = new UpdatePanel();
            contents.UpdateMode = UpdatePanelUpdateMode.Always;
            Controls.Add(contents);

            // generate body in update panel
            if (Form != null)
            {
                var rootVisualControl = CreateVisualControl(Form.RootVisual);
                if (rootVisualControl != null)
                    SetVisualControlId(rootVisualControl);

                // traps validation to ensure form is run
                validator = new CustomValidator();
                validator.ValidateEmptyText = true;
                validator.ValidationGroup = ValidationGroup;
                validator.ServerValidate += validator_ServerValidate;
                contents.ContentTemplateContainer.Controls.Add(validator);

                contents.ContentTemplateContainer.Controls.Add(rootVisualControl);
            }
        }

        private void validator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // ensure form is run before children are validated
            Form.Run();

            args.IsValid = true;
        }

        /// <summary>
        /// Navigates to the given <see cref="FormNavigation"/>.
        /// </summary>
        /// <param name="nav"></param>
        public void Navigate(FormNavigation nav)
        {
            FormPage newPage = null;

            // start at first navigation, if null
            if (nav == null)
                nav = Navigations.FirstOrDefault();

            if (nav is FormPage)
                // nav item is a page
                newPage = (FormPage)nav;
            else if (nav is FormSection)
                newPage = ((FormSection)nav).Descendants(false).OfType<FormPage>().FirstOrDefault();
            else
                throw new InvalidOperationException();

            // only change page if required
            if (CurrentPage != newPage)
                CurrentPage = newPage;
        }

        /// <summary>
        /// Returns the designated 'label' visual for the given <see cref="XFormsVisual"/> parent.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private XFormsLabelVisual GetLabelVisual(StructuralVisual visual)
        {
            return visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
        }

        /// <summary>
        /// Invoked for the PreRender phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            if (Form == null)
                return;

            if (!Visible)
                return;

            if (Page.Request.ServerVariables["HTTPS"] == "on")
                Page.ClientScript.RegisterClientScriptInclude("jq171", "https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js");
            else
                Page.ClientScript.RegisterClientScriptInclude("jq171", "http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FormView), "FormView_jQuery", "var FormView_jQuery = jQuery.noConflict();\n", true);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);

            // process any changes
            Form.Run();

            // prime unique ids required for rendering navigations
            Navigations
                .SelectMany(i => i.Descendants(true))
                .Select(i => i.Id)
                .ToList();
        }

        /// <summary>
        /// Renders the form.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (Form == null)
                return;

            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // render form and contents
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "FormView");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders a tree of navigation items.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="items"></param>
        private void RenderNavigations(HtmlTextWriter writer, IEnumerable<FormNavigation> items)
        {
            var relevantItems = items.Where(i => i.Relevant);

            // skip if no relevant descendants
            if (relevantItems.Any())
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var item in relevantItems)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    if (item is FormSection)
                        writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, Page.ClientScript.GetPostBackClientHyperlink(this, "navigate:" + item.Id));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                    }

                    var label = item.Visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
                    if (label != null)
                        FormHelper.RenderLabel(writer, label);
                    else
                        writer.WriteEncodedText("unknown");


                    writer.RenderEndTag();

                    // render children of navigation item
                    if (item is FormSection)
                        RenderNavigations(writer, ((FormSection)item).Children);

                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Initiates a submission of the form.
        /// </summary>
        public void Submit()
        {
            if (Form != null)
                Form.Submit();
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            var args = eventArgument.Split(':');

            switch (args[0])
            {
                case "navigate":
                    Navigate(Navigations
                        .SelectMany(i => i.Descendants(true))
                        .FirstOrDefault(i => i.Id == args[1]));
                    break;
            }
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var desc = new ScriptControlDescriptor("ISIS.Forms.Web.UI.FormView", ClientID);
            yield return desc;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("ISIS.Forms.Web.UI.FormView.js", typeof(FormView).Assembly.FullName);
        }
    }

}
