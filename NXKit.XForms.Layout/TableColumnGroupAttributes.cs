using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Provides the attributes for the 'table-column-group' element.
    /// </summary>
    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-column-group")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class TableColumnGroupAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public TableColumnGroupAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string Name
        {
            get { return GetAttributeValue("name"); }
        }

    }

}
