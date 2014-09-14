using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);

            this.attributes = attributes;
        }

        public string ColumnGroup
        {
            get { return attributes.Value.ColumnGroup; }
        }

    }

}
