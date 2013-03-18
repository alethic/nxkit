using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NXKit.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    public class TableColumnGroupCollection : IEnumerable<TableColumnGroup>
    {

        TableColumnGroup[] columnGroups;
        Dictionary<string, TableColumnGroup> columnGroupsByName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TableColumnGroupCollection(View view, Table table)
        {
            View = view;
            Table = table;
            Table.Visual.AddEventHandler<VisualAddedEvent>(VisualAddedHandler, false);
        }

        public View View { get; private set; }

        public Table Table { get; private set; }

        /// <summary>
        /// Invoked when a new visual is added underneath this table.
        /// </summary>
        /// <param name="ev"></param>
        void VisualAddedHandler(Event ev)
        {
            var @event = ev as VisualAddedEvent;
            if (@event == null)
                return;

            columnGroups = null;
            columnGroupsByName = null;
        }

        /// <summary>
        /// Yields the set of column groups that make up this column group collection.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TableColumnGroup> GetColumnGroups()
        {
            var visuals = View.OpaqueChildren(Table.Visual).OfType<TableColumnGroupVisual>();
            if (visuals.Any())
                foreach (var item in visuals.Select((i, j) => new TableColumnGroup(View, Table, j, i)))
                    yield return item;
            else
                yield return new TableColumnGroup(View, Table, 0, null);
        }

        /// <summary>
        /// Ensures the column groups have been initialized.
        /// </summary>
        void EnsureColumnGroups()
        {
            if (columnGroups == null)
                columnGroups = GetColumnGroups().ToArray();
        }

        public TableColumnGroup this[int index]
        {
            get
            {
                EnsureColumnGroups();

                if (index >= columnGroups.Length)
                    throw new IndexOutOfRangeException();

                return columnGroups[index];
            }
        }

        public TableColumnGroup this[string name]
        {
            get
            {
                EnsureColumnGroups();

                // create dictionary if not already created
                if (columnGroupsByName == null)
                    columnGroupsByName = columnGroups.ToDictionary(i => i.Visual != null && i.Visual.Name != null ? i.Visual.Name : "", i => i);

                return name != null ? columnGroupsByName[name] : columnGroups[0];
            }
        }

        public int Length
        {
            get { EnsureColumnGroups(); return columnGroups.Length; }
        }

        #region IEnumerable<TableColumnGroup> Members

        public IEnumerator<TableColumnGroup> GetEnumerator()
        {
            EnsureColumnGroups();

            foreach (var columnGroup in columnGroups)
                yield return columnGroup;
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
