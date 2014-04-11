using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides extension methods for <see cref="XNode"/> instances.
    /// </summary>
    public static class XNodeExtensions
    {

        /// <summary>
        /// Gets the unique identifier for the node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static int GetNodeId(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Document != null);

            // gets the node id, or allocates a new one with the document
            return node.AnnotationOrCreate<NodeAnnotation>(() =>
                new NodeAnnotation(
                    node.Document.AnnotationOrCreate<DocumentAnnotation>()
                        .GetNextNodeId())).Id;
        }


    }

}
