using System.Collections;
using System.Collections.Generic;
using System.Linq;

using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    public class TableRowRowCollection : IEnumerable<TableRow>
    {

        private TableRow[] rows;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TableRowRowCollection(TableRow parent)
        {
            Parent = parent;
        }

        public TableRow Parent { get; private set; }

        /// <summary>
        /// Iterates each child TableRow. Descends into non-TableRows.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private static IEnumerable<TableRowVisual> GetRows(StructuralVisual visual)
        {
            foreach (var child in visual.Children)
                if (child is TableRowVisual)
                    yield return (TableRowVisual)child;
                else if (child is StructuralVisual)
                    foreach (var child2 in GetRows((StructuralVisual)child))
                        yield return child2;
        }

        /// <summary>
        /// Yields the set of column groups that make up this column group collection.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TableRow> GetRows()
        {
            int i = 0;

            foreach (var row in GetRows(Parent.Visual))
                yield return new TableRow(Parent.Table, ++i, Parent.Depth + 1, row);
        }

        /// <summary>
        /// Ensures the rows have been initialized.
        /// </summary>
        private void EnsureRows()
        {
            if (rows == null)
                rows = GetRows().ToArray();
        }

        public TableRow this[int i]
        {
            get { EnsureRows(); return rows[i]; }
        }

        public int Length
        {
            get { EnsureRows(); return rows.Length; }
        }

        #region IEnumerable<TableRow> Members

        public IEnumerator<TableRow> GetEnumerator()
        {
            EnsureRows();

            foreach (var row in rows)
                yield return row;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }

}
