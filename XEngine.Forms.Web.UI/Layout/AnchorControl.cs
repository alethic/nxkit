using System;
using System.Web.UI;

using XEngine.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    [VisualControlTypeDescriptor]
    public class AnchorControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is AnchorVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new AnchorControl(view, (AnchorVisual)visual);
        }

    }

    public class AnchorControl : VisualContentControl<AnchorVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public AnchorControl(FormView view, AnchorVisual visual)
            : base(view, visual)
        {

        }

        protected override VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual, includeTextAsContent: true);
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (!string.IsNullOrEmpty(Visual.Href))
            {
                Uri uri;
                if (Uri.TryCreate(Visual.Href, UriKind.Absolute, out uri))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, uri.ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                }
                else if (Uri.TryCreate(Visual.Href, UriKind.Relative, out uri))
                {
                    var resolvedUriStr = View.ResolveResourceClientUrl(uri.ToString(), null);
                    if (resolvedUriStr != null && Uri.TryCreate(resolvedUriStr, UriKind.RelativeOrAbsolute, out uri))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, uri.ToString());
                        writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                    }
                }
            }

            writer.RenderBeginTag(HtmlTextWriterTag.A);
            base.Render(writer);
            writer.RenderEndTag();
        }

    }

}
