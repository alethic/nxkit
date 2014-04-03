using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    public class NXCData :
        NXText
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXCData()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public NXCData(string value)
            : base(value)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NXCData(XCData xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
        }

    }

}
