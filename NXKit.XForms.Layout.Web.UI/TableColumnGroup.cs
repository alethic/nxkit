using NXKit.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    public class TableColumnGroup
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableColumnGroup(View view, Table table, int index, TableColumnGroupVisual visual)
        {
            View = view;
            Table = table;
            Index = index;
            Visual = visual;
            //ColumnGroups = new TableColumnGroupCollection(view, visual);
            Columns = new TableColumnCollection(view, this, visual);
        }

        public View View { get; private set; }

        public Table Table { get; private set; }

        public TableColumnGroupVisual Visual { get; private set; }

        public TableColumnGroupCollection ColumnGroups { get; private set; }

        public TableColumnCollection Columns { get; private set; }

        public int Index { get; private set; }

    }

}
