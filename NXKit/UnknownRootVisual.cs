using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides an unknown visual.
    /// </summary>
    class UnknownRootVisual :
        NXElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        internal UnknownRootVisual(XElement xml)
            : base(xml)
        {
            Contract.Requires<ArgumentNullException>(xml != null);
        }

        public override string Id
        {
            get { return Document.GetElementId(Xml); }
        }

    }

}
