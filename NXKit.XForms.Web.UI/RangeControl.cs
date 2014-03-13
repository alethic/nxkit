using System;
using System.Diagnostics.Contracts;
using System.Web.UI;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class RangeControlDescriptor : 
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsRangeVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new RangeControl(view, (XFormsRangeVisual)visual);
        }

    }

    public class RangeControl : 
        VisualControl<XFormsRangeVisual>
    {

        static readonly IRangeEditableProvider editableProvider =
            new DefaultRangeEditableProvider();

        Control ctl;
        VisualControl lbl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public RangeControl(View view, XFormsRangeVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void CreateChildControls()
        {
            ctl = editableProvider.Create(View, Visual);
            ctl.ID = Visual.Type != null ? Visual.Type.LocalName : "default";
            Controls.Add(ctl);

            var lblVisual = Visual.FindLabelVisual();
            if (lblVisual != null)
            {
                lbl = new LabelControl(View, lblVisual);
                lbl.ID = "lbl";
                Controls.Add(lbl);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-range");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (lbl != null)
            {
                // target control if it can be targeted
                if (ctl is IFocusTarget)
                    writer.AddAttribute(HtmlTextWriterAttribute.For, ((IFocusTarget)ctl).TargetID);

                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                lbl.RenderControl(writer);
                writer.RenderEndTag();
            }

            ctl.RenderControl(writer);

            writer.RenderEndTag();
        }

    }

}
