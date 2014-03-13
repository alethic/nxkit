using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class OutputControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsOutputVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new OutputControl(view, (XFormsOutputVisual)visual);
        }

    }

    public class OutputControl :
        VisualControl<XFormsOutputVisual>
    {

        static readonly IOutputViewProvider viewProvider =
            new DefaultOutputViewProvider();

        Control ctl;
        VisualControl lbl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public OutputControl(View view, XFormsOutputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void CreateChildControls()
        {
            ctl = CreateOutputControl(Visual);
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

        /// <summary>
        /// Creates an output control based on the bound data type.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control CreateOutputControl(XFormsOutputVisual visual)
        {
            return viewProvider.Create(View, visual);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-output");
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
