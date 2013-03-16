using XEngine.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public class Table
    {

        public Table(TableVisual visual)
        {
            Visual = visual;
            ColumnGroups = new TableColumnGroupCollection(this);
            Rows = new TableRowCollection(this);
        }

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
