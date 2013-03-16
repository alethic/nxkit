using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public class TableRow
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableRow(Table table, int position, int depth, TableRowVisual visual)
        {
            Table = table;
            Position = position;
            Depth = depth;
            Visual = visual;
            Cells = new TableCellCollection(this);
            Rows = new TableRowRowCollection(this);
        }

        public Table Table { get; private set; }

        public TableRowVisual Visual { get; private set; }

        /// <summary>
        /// Gets the <see cref="ColumnGroup"/> this row is a member of.
        /// </summary>
        public TableColumnGroup ColumnGroup
        {
            get { return Visual.ColumnGroup != null ? Table.ColumnGroups[Visual.ColumnGroup] : Table.ColumnGroups[0]; }
        }

        /// <summary>
        /// Gets the position of the row within its parent.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the current depth level of this row.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// Gets the set of cells that make up this row.
        /// </summary>
        public TableCellCollection Cells { get; private set; }

        /// <summary>
        /// Gets the set of runs nested underneath this row.
        /// </summary>
        public TableRowRowCollection Rows { get; private set; }

    }

}
