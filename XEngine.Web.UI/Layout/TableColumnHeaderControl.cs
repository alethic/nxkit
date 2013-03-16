using System.Web.UI;

using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    [VisualControlTypeDescriptor]
    public class TableColumnHeaderControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is TableColumnVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new TableColumnHeaderControl(view, (TableColumnVisual)visual);
        }

    }

    public class TableColumnHeaderControl : VisualContentControl<TableColumnVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableColumnHeaderControl(FormView view, TableColumnVisual visual)
            : base(view, visual)
        {

        }

        protected override VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual, includeTextAsContent: true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Visual.Relevant)
                return;

            base.Render(writer);
        }

    }

}
