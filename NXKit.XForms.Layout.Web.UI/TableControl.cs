using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.UI;

using NXKit.Web.UI;
using NXKit.XForms.Web.UI;

namespace NXKit.XForms.Layout.Web.UI
{

    [VisualControlTypeDescriptor]
    public class TableControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is TableVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new TableControl(view, (TableVisual)visual);
        }

    }

    public class TableControl : 
        SingleNodeBindingVisualControl<TableVisual>, 
        IScriptControl
    {

        readonly Table model;
        TableControlColumnHeaderControlCollection columnHeaderControls;
        TableControlCellControlCollection cellControls;
        TriggerControlCollection triggerControls;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TableControl(View view, TableVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual!= null);

            this.model = new Table(View, visual);
        }

        public Table Model
        {
            get { return model; }
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Add(columnHeaderControls = new TableControlColumnHeaderControlCollection(View, this));
            Controls.Add(cellControls = new TableControlCellControlCollection(View, this));
            Controls.Add(triggerControls = new TriggerControlCollection(View, Visual));
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
        }

        /// <summary>
        /// Renders the HTML colgroup tag for a given Column Group table.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="columnGroup"></param>
        private void RenderColGroup(HtmlTextWriter writer, TableColumnGroup columnGroup)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Colgroup);
            for (int colIndex = 0; colIndex < columnGroup.Columns.Length; colIndex++)
            {
                var column = columnGroup.Columns[colIndex];

                int width = 100 / columnGroup.Columns.Length;
                if (colIndex == columnGroup.Columns.Length - 1)
                    width += 100 % columnGroup.Columns.Length;

                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, width + "%");
                writer.RenderBeginTag(HtmlTextWriterTag.Col);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Visual.Relevant)
                return;

            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // build table css
            var tableCss = new StringBuilder("Layout_Table ")
                .Append(LayoutHelpers.ImportanceCssClass(Visual.Importance));

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, tableCss.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            for (int columnGroupIndex = 0; columnGroupIndex < Model.ColumnGroups.Length; columnGroupIndex++)
            {
                var columnGroup = Model.ColumnGroups[columnGroupIndex];

                // skip group if no columns with headers
                if (!columnGroup.Columns.Any(i => i.Visual != null && i.Visual.Visuals.Any()))
                    continue;

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "h g" + columnGroupIndex);
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                // render colgroup tag
                RenderColGroup(writer, columnGroup);

                writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                for (int cellIndex = 0; cellIndex < columnGroup.Columns.Length; cellIndex++)
                {
                    var column = columnGroup.Columns[cellIndex];

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "i" + cellIndex);
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);

                    // render individual cell contents
                    var ctl = column.Visual != null ? columnHeaderControls.GetOrCreateControl(column.Visual) : null;
                    if (ctl != null)
                        ctl.RenderControl(writer);

                    writer.RenderEndTag();
                }

                // end column group
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            // render all rows
            RenderRows(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Triggers");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            triggerControls.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private IEnumerable<TableRow> GetTableRows(IEnumerable<TableRow> rows)
        {
            // child rows of parent
            foreach (var row in rows)
            {
                yield return row;

                // yield each child row with increasing depth
                foreach (var row2 in GetTableRows(row.Rows))
                    yield return row2;
            }
        }

        private void RenderRows(HtmlTextWriter writer)
        {
            TableColumnGroup columnGroup = null;

            // flatten row hierarchy
            foreach (var row in GetTableRows(Model.Rows))
            {
                // skip non-relevant rows; rows might be nested in a group
                if (!row.Visual.Relevant)
                    continue;

                // new group
                if (columnGroup != row.ColumnGroup)
                {
                    // end old group
                    if (columnGroup != null)
                    {
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }

                    // set new group
                    columnGroup = row.ColumnGroup;

                    // render group opening
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "g" + columnGroup.Index);
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    // render colgroup tag
                    RenderColGroup(writer, columnGroup);

                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                }

                // render row
                RenderRow(writer, row);
            }

            // end last group
            if (columnGroup != null)
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }

        private void RenderRow(HtmlTextWriter writer, TableRow row)
        {
            // build row css
            var rowCss = new StringBuilder()
                .Append("i").Append(row.Position).Append(" ")
                .Append("d").Append(row.Depth).Append(" ");

            // append alternate classes
            rowCss.Append("nth_2n").Append(row.Position % 2).Append(" ");
            rowCss.Append("nth_3n").Append(row.Position % 3).Append(" ");

            if (row.Visual != null)
                rowCss.Append(LayoutHelpers.ImportanceCssClass(row.Visual.Importance)).Append(" ");

            writer.AddAttribute(HtmlTextWriterAttribute.Class, rowCss.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            int lastCellIndex = 0;

            // render each cell
            foreach (var cell in row.Cells)
            {
                var ctl = cellControls.GetOrCreateControl(cell.Visual);
                if (ctl != null)
                {
                    // build cell css
                    var cellCss = new StringBuilder()
                        .Append("i").Append(cell.Column.Index).Append(" ");

                    if (cell.Column.Visual != null)
                        cellCss.Append(LayoutHelpers.ImportanceCssClass(cell.Column.Visual.Importance));

                    if (cell.Visual != null)
                        cellCss.Append(LayoutHelpers.ImportanceCssClass(cell.Visual.Importance));

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, cellCss.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    // render individual cell contents
                    ctl.RenderControl(writer);

                    writer.RenderEndTag();
                }

                lastCellIndex = cell.Column.Index;
            }

            // end row
            writer.RenderEndTag();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var desc = new ScriptControlDescriptor("NXKit.XForms.Layout.Web.UI.TableControl", ClientID);
            desc.AddComponentProperty("view", View.ClientID);
            yield return desc;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("NXKit.XForms.Layout.Web.UI.TableControl.js", typeof(TableControl).Assembly.FullName);
        }

    }

}
