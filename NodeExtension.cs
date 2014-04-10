using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base class for an extension on <see cref="XNode"/> instances.
    /// </summary>
    public abstract class NodeExtension
    {

        readonly XNode node;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public NodeExtension(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
        }

        /// <summary>
        /// Gets the <see cref="XNode"/> this extension applies to.
        /// </summary>
        public XNode Node
        {
            get { return node; }
        }

    }

}
