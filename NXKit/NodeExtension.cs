using System;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base class for an extension on <see cref="XNode"/> instances.
    /// </summary>
    public abstract class NodeExtension :
        IExtension
    {

        readonly XNode node;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public NodeExtension(XNode node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public XObject Object
        {
            get { return node; }
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
