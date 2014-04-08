using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Provides the attributes for the 'table-row' element.
    /// </summary>
    public class TableRowAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableRowAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string ColumnGroup
        {
            get { return GetAttributeValue("column-group"); }
        }

    }

}
