using XEngine.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public class TableColumn
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableColumn(TableColumnGroup columnGroup ,int index,TableColumnVisual visual)
        {
            ColumnGroup = columnGroup;
            Index = index;
            Visual = visual;
        }

        public TableColumnGroup ColumnGroup { get; private set; }

        public int Index { get; private set;}

        public TableColumnVisual Visual { get; private set; }

    }

}
