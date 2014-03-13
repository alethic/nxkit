using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides an unknown visual.
    /// </summary>
    class UnknownRootVisual :
        ContentVisual,
        IVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        internal UnknownRootVisual(INXDocument engine, ContentVisual parent, XNode node)
            : base(engine, parent, node)
        {
            Contract.Requires<ArgumentNullException>(engine != null);
        }

        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

    }

}
