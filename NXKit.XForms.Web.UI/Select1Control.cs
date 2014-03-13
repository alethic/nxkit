using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class Select1ControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsSelect1Visual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new Select1Control(view, (XFormsSelect1Visual)visual);
        }

    }

    public class Select1Control :
        SingleNodeBindingVisualControl<XFormsSelect1Visual>
    {

        static readonly ISelect1EditableProvider editableProvider =
            new DefaultSelect1EditableProvider();

        Control ctl;
        CommonControlCollection common;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1Control(View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void CreateChildControls()
        {
            Controls.Add(ctl = editableProvider.Create(View, Visual));
            ctl.ID = "ctl";

            Controls.Add(common = new CommonControlCollection(View, Visual));
            common.ID = "common";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-select1");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (common.LabelControl != null)
            {
                // target control if it can be targeted
                if (ctl is IFocusTarget)
                    writer.AddAttribute(HtmlTextWriterAttribute.For, ((IFocusTarget)ctl).TargetID);

                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            ctl.RenderControl(writer);

            writer.RenderEndTag();
        }

    }

}
