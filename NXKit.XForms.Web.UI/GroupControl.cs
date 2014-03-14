using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.UI;

using NXKit.Util;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class GroupControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual.GetType() == typeof(XFormsGroupVisual);
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new GroupControl(view, (XFormsGroupVisual)visual);
        }

    }

    public class GroupControl :
        SingleNodeBindingVisualContentControl<XFormsGroupVisual>
    {

        CommonControlCollection common;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public GroupControl(View view, XFormsGroupVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(common = new CommonControlCollection(View, Visual));
            common.ID = "common";
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
            // obtain additional css classes if available
            var args = new BeginRenderEventArgs();
            OnBeginRender(args);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Join(" ", args.CssClasses.Distinct().Prepend("xforms-group")));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (common.LabelControl != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-label");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            base.Render(writer);

            writer.RenderEndTag();
        }

    }

}
