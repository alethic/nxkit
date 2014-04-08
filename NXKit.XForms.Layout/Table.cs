using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Interface("{http://schemas.nxkit.org/nxkit/2014/xforms-layout}table")]
    public class Table :
        ITableColumnGroupContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Table(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
