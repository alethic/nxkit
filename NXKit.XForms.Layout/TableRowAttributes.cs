using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Provides the attributes for the 'table-row' element.
    /// </summary>
    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}table-row")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class TableRowAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public TableRowAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        public string ColumnGroup
        {
            get { return GetAttributeValue("column-group"); }
        }

    }

}
