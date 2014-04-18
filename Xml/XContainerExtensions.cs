using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Xml
{

    public static class XContainerExtensions
    {

        public static IEnumerable<XNode> DescendantNodesAndSelf(this XContainer self)
        {
            yield return self;

            foreach (var node in self.DescendantNodes())
                yield return node;
        }

        /// <summary>
        /// Returns a collection of nodes that contain this element, and all decendant nodes of this element, in 
        /// document order. If <paramref name="safe"/> is <c>true</c>, the returned node can be deleted while it is
        /// the current node without breaking the enumerator.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="lookahead"></param>
        /// <returns></returns>
        public static IEnumerable<XNode> DescendantNodesAndSelf(this XContainer self, bool safe)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return safe ? DescendantNodesAndSelfSafe(self) : DescendantNodesAndSelf(self);
        }

        /// <summary>
        /// Returns a collection of nodes that contain this element, and all decendant nodes of this element, in
        /// document order, with single lookahead to allow deletion of the active node.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static IEnumerable<XNode> DescendantNodesAndSelfSafe(XContainer self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var node = (XNode)self;
            if (node != null)
            {
                var next = node.NextNode;

                do
                {
                    yield return node;

                    if (node.Document != null)
                    {
                        next = node.NextNode;

                        if (node is XContainer)
                            foreach (var step in ((XContainer)node).Nodes())
                                foreach (var step2 in DescendantNodesAndSelfSafe(step))
                                    yield return step2;
                    }
                }
                while ((node = next) != null);
            }

        static IEnumerable<XNode> DescendantNodesSafe(XContainer container)
        {

        }

    }

}
