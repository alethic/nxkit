using NXKit.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    public class Table
    {

        public Table(View view, TableVisual visual)
        {
            View = view;
            Visual = visual;
            ColumnGroups = new TableColumnGroupCollection(View, this);
            Rows = new TableRowCollection(this);
        }

        public View View { get; private set; }

        public TableVisual Visual { get; private set; }

        /// <summary>
        /// Gets the collection of column groups.
        /// </summary>
        public TableColumnGroupCollection ColumnGroups { get; private set; }

        /// <summary>
        /// Gets the collection of top-level rows.
        /// </summary>
        public TableRowCollection Rows { get; private set; }

    }

}
