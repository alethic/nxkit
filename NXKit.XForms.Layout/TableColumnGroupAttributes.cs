using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Provides the attributes for the 'table-column-group' element.
    /// </summary>
    public class TableColumnGroupAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
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
