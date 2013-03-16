using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ISIS.Forms.Layout;
using System;

namespace ISIS.Forms.Web.UI.Layout
{

    public class TableColumnCollection : IEnumerable<TableColumn>
    {

        private List<TableColumn> columns;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public TableColumnCollection(TableColumnGroup columnGroup, TableColumnGroupVisual visual)
        {
            ColumnGroup = columnGroup;
            Visual = visual;
        }

        public TableColumnGroup ColumnGroup { get; private set; }

        public TableColumnGroupVisual Visual { get; private set; }

        /// <summary>
        /// Yields the set of columns that make up this column collection.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TableColumn> GetColumns()
        {
            int index = 0;

            var columnVisuals = Visual != null ? Visual.OpaqueChildren().OfType<TableColumnVisual>() : Enumerable.Empty<TableColumnVisual>();
            if (columnVisuals.Any())
                foreach (var columnVisual in columnVisuals)
                    yield return new TableColumn(ColumnGroup, index++, columnVisual);

            // determinist the maximum number of cells
            int maxCellCount = index;

            foreach (var rowVisual in RowVisuals())
            {
                // find count of cells in row
                int rowCellCount = CellVisuals(rowVisual).Count();
                if (rowCellCount > maxCellCount)
                    maxCellCount = rowCellCount;
            }

            // generate new columns for when more cells
            for (; index < maxCellCount; index++)
                yield return new TableColumn(ColumnGroup, index, null);
        }

        private IEnumerable<TableCellVisual> CellVisuals(TableRowVisual rowVisual)
        {
            foreach (var cellVisual in rowVisual.OpaqueChildren().OfType<TableCellVisual>())
                yield return cellVisual;
        }

        private IEnumerable<TableRowVisual> RowVisuals()
        {
            foreach (var rowVisual in ColumnGroup.Table.Visual.OpaqueChildren().OfType<TableRowVisual>())
            {
                // automatically generated column group
                if (ColumnGroup.Visual == null)
                    if (string.IsNullOrEmpty(rowVisual.ColumnGroup))
                    {
                        yield return rowVisual;
                        continue;
                    }

                // column group matches
                if (ColumnGroup.Visual.Name == rowVisual.ColumnGroup)
                {
                    yield return rowVisual;
                    continue;
                }
            }
        }

        /// <summary>
        /// Ensures the columns have been initialized.
        /// </summary>
        private void EnsureColumns()
        {
            if (columns == null)
                columns = GetColumns().ToList();
        }

        public TableColumn this[int index]
        {
            get
            {
                EnsureColumns();

                while (index >= columns.Count)
                    columns.Add(new TableColumn(ColumnGroup, columns.Count, null));

                return columns[index];
            }
        }

        public int Length
        {
            get { EnsureColumns(); return columns.Count; }
        }

        #region IEnumerable<TableColumn> Members

        public IEnumerator<TableColumn> GetEnumerator()
        {
            EnsureColumns();

            foreach (var column in columns)
                yield return column;
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
