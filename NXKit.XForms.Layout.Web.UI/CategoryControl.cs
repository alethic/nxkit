using System;
using System.Linq;

using NXKit.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    [VisualControlTypeDescriptor]
    public class CategoryControlDescriptor : VisualControlTypeDescriptor
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

    public class CategoryControl : VisualContentControl<CategoryVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public CategoryControl(View view, CategoryVisual visual)
            : base(view, visual)
        {

        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            foreach (var page in View.OpaqueChildren(Visual).OfType<PageVisual>())
            {
                // only make current page visible
                var ctl = Content.GetOrCreateControl(page);
                if (ctl != null)
                    ctl.Visible = View.CurrentPage.Visual == page;
            }
        }

    }

}
