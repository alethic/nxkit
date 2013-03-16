using ISIS.Forms.Layout;

namespace ISIS.Forms.Web.UI.Layout
{

    public class TableColumnGroup
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableColumnGroup(Table table, int index, TableColumnGroupVisual visual)
        {
            Table = table;
            Index = index;
            Visual = visual;
            Columns = new TableColumnCollection(this, visual);
        }

        public Table Table { get; private set; }

        public TableColumnGroupVisual Visual { get; private set; }

        public TableColumnCollection Columns { get; private set; }

        public int Index { get; private set; }

    }

}
