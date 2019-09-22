using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-row")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class TableRow :
        ElementExtension
    {

        readonly Extension<TableRowAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public TableRow(
            XElement element,
            Extension<TableRowAttributes> attributes)
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
