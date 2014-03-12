using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.UI;
using NXKit.Web.UI;
using NXKit.XForms.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    [VisualControlTypeDescriptor]
    public class PageControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is PageVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new PageControl(view, (PageVisual)visual);
        }

    }

    public class PageControl :
        SingleNodeBindingVisualContentControl<PageVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public PageControl(View view, PageVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);

            View.CurrentPageChanged += View_CurrentPageChanged;
        }

        void View_CurrentPageChanged(object sender, EventArgs args)
        {
            Refresh();
        }

        protected override void OnLoadComplete(EventArgs args)
        {
            base.OnLoadComplete(args);

            Refresh();
        }

        protected override void OnVisualEnabled()
        {
            base.OnVisualEnabled();

            Refresh();
        }

        protected override void OnVisualDisabled()
        {
            base.OnVisualDisabled();

            Refresh();
        }

        /// <summary>
        /// Recalculates the state of the page control.
        /// </summary>
        void Refresh()
        {
            // find ourselves, and check if we're the active relevant page
            Visible = View.Navigations
                .Where(i => i.Visual is PageVisual)
                .Where(i => i.Visual == Visual)
                .Where(i => i.Visual == View.CurrentPage.Visual)
                .Where(i => i.Visual.Relevant)
                .Any();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Layout_Page");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

    }

}
