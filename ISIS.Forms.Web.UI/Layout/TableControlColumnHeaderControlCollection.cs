using System.Collections.Generic;
using System.Linq;

using ISIS.Util;

namespace ISIS.Forms.Web.UI.Layout
{

    public class TableControlColumnHeaderControlCollection : VisualControlCollection
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TableControlColumnHeaderControlCollection(FormView view, TableControl parent)
            : base(view, parent.Visual)
        {
            Table = parent.Model;
        }

        private Table Table { get; set; }

        protected override IEnumerable<Visual> GetVisuals()
        {
            // extract column visuals from table's model
            return Table.ColumnGroups
                .SelectMany(i => i.Columns)
                .Select(i => i.Visual)
                .NotNull();
        }

    }

}
