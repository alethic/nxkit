using System;
using System.Linq;
using System.Web.UI;

using NXKit.XForms;

namespace NXKit.XForms.Web.UI.XForms
{

    [VisualControlTypeDescriptor]
    public class GroupControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual.GetType() == typeof(XFormsGroupVisual);
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new GroupControl(view, (XFormsGroupVisual)visual);
        }

    }

    public class GroupControl : VisualContentControl<XFormsGroupVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public GroupControl(FormView view, XFormsGroupVisual visual)
            : base(view, visual)
        {

        }

        public CommonControlCollection Common { get; private set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(Common = new CommonControlCollection(View, Visual));
        }

        /// <summary>
        /// Raised when the control has begun to render.
        /// </summary>
        public event EventHandler<BeginRenderEventArgs> BeginRender;

        /// <summary>
        /// Raises the BeginRender event.
        /// </summary>
        /// <param name="args"></param>
        private void OnBeginRender(BeginRenderEventArgs args)
        {
            if (BeginRender != null)
                BeginRender(this, args);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Visual.Relevant)
                return;

            // obtain additional css classes if available
            var args = new BeginRenderEventArgs();
            OnBeginRender(args);
            var classAttr = string.Join(" ", args.CssClasses.Distinct());

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Group " + classAttr);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (Common.LabelControl != null)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                Common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            if (Visual.Appearance == NXKit.Layout.Constants.Layout_1_0 + "sequence")
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Layout_Sequence");
                writer.RenderBeginTag(HtmlTextWriterTag.Ol);

                foreach (Control control in Content.Controls)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    control.RenderControl(writer);
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
            }
            else
                base.Render(writer);

            writer.RenderEndTag();
        }

    }

}
