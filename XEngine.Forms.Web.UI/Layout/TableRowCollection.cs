using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NXKit.Layout;

namespace NXKit.XForms.Web.UI.Layout
{

    public class TableRowCollection : IEnumerable<TableRow>
    {

        private TableRow[] rows;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TableRowCollection(Table table)
        {
            Table = table;
            Table.Visual.AddEventHandler<VisualAddedEvent>(VisualAddedHandler, false);
        }

        public Table Table { get; private set; }

        /// <summary>
        /// Invoked when a new visual is added underneath this table.
        /// </summary>
        /// <param name="ev"></param>
        private void VisualAddedHandler(Event ev)
        {
            var @event = ev as VisualAddedEvent;
            if (@event == null)
                return;

            rows = null;
        }

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
        /// Yields the set of top-level rows that make up this table.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TableRow> GetRows()
        {
            int i = 0;

            foreach (var row in GetRows(Table.Visual))
                yield return new TableRow(Table, ++i, 0, row);
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
