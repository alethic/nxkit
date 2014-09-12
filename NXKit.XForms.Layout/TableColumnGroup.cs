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
        ITableColumnGroupContainer
    {

        readonly XElement element;
        readonly TableColumnGroupAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumnGroup(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new TableColumnGroupAttributes(element);
        }

        public string Name
        {
            get { return attributes.Name; }
        }

    }

}
