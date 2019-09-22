using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-column-group")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class TableColumnGroup :
        ElementExtension,
        ITableColumnGroupContainer
    {

        readonly Extension<TableColumnGroupAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public TableColumnGroup(
            XElement element,
            Extension<TableColumnGroupAttributes> attributes)
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
