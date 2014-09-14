using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);

            this.attributes = attributes;
        }

        public string Name
        {
            get { return attributes.Value.Name; }
        }

    }

}
