using System;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-row")]
    public class TableRow :
        ElementExtension
    {

        readonly Lazy<TableRowAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableRow(
            XElement element,
            Lazy<TableRowAttributes> attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        public string ColumnGroup
        {
            get { return attributes.Value.ColumnGroup; }
        }

    }

}
