using System.Web.UI;

using XEngine.Forms.Layout;
using XEngine.Forms.Web.UI.XForms;

namespace XEngine.Forms.Web.UI.Layout
{

    [VisualControlTypeDescriptor]
    public class SectionControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is SectionVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new SectionControl(view, (SectionVisual)visual);
        }

    }

    public class SectionControl : VisualContentControl<SectionVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public SectionControl(FormView view, SectionVisual visual)
            : base(view, visual)
        {

        }

        public CommonControlCollection Common { get; private set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(Common = new CommonControlCollection(View, Visual));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Visual.Relevant)
                return;

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Layout_Section");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (Common.LabelControl != null)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                Common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            base.Render(writer);

            writer.RenderEndTag();
        }

    }

}
