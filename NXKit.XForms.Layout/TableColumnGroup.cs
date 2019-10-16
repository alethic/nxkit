using System;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-column-group")]
    public class TableColumnGroup :
        ElementExtension,
        ITableColumnGroupContainer
    {

        readonly Lazy<TableColumnGroupAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public TableColumnGroup(
            XElement element,
            Lazy<TableColumnGroupAttributes> attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        public string Name
        {
            get { return attributes.Value.Name; }
        }

    }

}
