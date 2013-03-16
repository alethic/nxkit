using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public class TableCell
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableCell(TableRow row, TableColumn column, TableCellVisual visual)
        {
            Row = row;
            Column = column;
            Visual = visual;
        }

        public TableRow Row { get; private set; }

        public TableColumn Column { get; private set; }

        public TableCellVisual Visual { get; private set; }

    }

}
