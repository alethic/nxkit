using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Xml
{

    public static class XContainerExtensions
    {

        /// <summary>
        /// Returns a collection of the descendant nodes for this container.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<XNode> DescendantNodesAndSelf(this XContainer self)
        {
            yield return self;

            foreach (var node in self.DescendantNodes())
                yield return node;
        }

        /// <summary>
        /// Clones the specified <see cref="XContainer"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XContainer Clone(this XContainer self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
