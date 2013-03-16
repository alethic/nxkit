using System.Collections;
using System.Collections.Generic;
using System.Linq;

using XEngine.Forms.Layout;
using System;

namespace XEngine.Forms.Web.UI.Layout
{

    public class TableCellCollection : IEnumerable<TableCell>
    {

        private TableCell[] cells;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TableCellCollection(TableRow row)
        {
            Row = row;
        }

        public TableRow Row { get; private set; }

        /// <summary>
        /// Iterates each child TableRow. Descends into non-TableRows.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private static IEnumerable<TableCellVisual> GetCells(StructuralVisual visual)
        {
            foreach (var child in visual.Children)
                if (child is TableCellVisual)
                    yield return (TableCellVisual)child;
                else if (child is TableRowVisual)
                    continue;
                else if (child is StructuralVisual)
                    foreach (var child2 in GetCells((StructuralVisual)child))
                        yield return child2;
        }

        /// <summary>
        /// Gets an iterator that generates <see cref="TableCell"/>s.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TableCell> GetCells()
        {
            int i = 0;
            foreach (var cell in GetCells(Row.Visual))
                yield return new TableCell(Row, Row.ColumnGroup.Columns[i++], cell);
        }

        /// <summary>
        /// Ensures the cells have been initialized.
        /// </summary>
        private void EnsureCells()
        {
            if (cells == null)
                cells = GetCells().ToArray();
        }

        public TableCell this[int index]
        {
            get
            {
                EnsureCells();

                if (index >= cells.Length)
                    throw new IndexOutOfRangeException();

                return cells[index];
            }
        }

        public int Length
        {
            get { EnsureCells(); return cells.Length; }
        }

        #region IEnumerable<TableCell> Members

        public IEnumerator<TableCell> GetEnumerator()
        {
            EnsureCells();

            foreach (var cell in cells)
                yield return cell;
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
