using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using NXKit.Web.UI;
using NXKit.XForms.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    [VisualControlTypeDescriptor]
    public class CategoryControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is CategoryVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new CategoryControl(view, (CategoryVisual)visual);
        }

    }

    public class CategoryControl :
        VisualContentControl<CategoryVisual>
    {

        CommonControlCollection common;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public CategoryControl(View view, CategoryVisual visual)
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

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-layout-category");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (common.LabelControl != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-label");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                common.LabelControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            if (common.HintControl != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "xforms-hint");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                common.HintControl.RenderControl(writer);
                writer.RenderEndTag();
            }

            base.Render(writer);

            writer.RenderEndTag();
        }

    }

}
