using System.Collections.Generic;
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

    }

}
