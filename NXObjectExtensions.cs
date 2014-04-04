using System.Collections.Generic;
using System.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="NXObject"/> instances.
    /// </summary>
    public static class NXObjectExtensions
    {

        /// <summary>
        /// Gets all ancestor container nodes of the given <see cref="NXObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXContainer> Ancestors(this NXObject self)
        {
            var node = self.Parent;
            while (node != null)
            {
                yield return node;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Gets all ancestor objects of the given <see cref="NXObject"/>, including itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXObject> AncestorsAndSelf(this NXObject self)
        {
            return Ancestors(self).Prepend(self);
        }

        /// <summary>
        /// Gets all ancestor nodes of the given <see cref="NXNode"/>, including itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXNode> AncestorsAndSelf(this NXNode self)
        {
            return Ancestors(self).Prepend(self);
        }

        /// <summary>
        /// Gets all ancestor containers of the given <see cref="NXContainer"/>, including itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXContainer> AncestorsAndSelf(this NXContainer self)
        {
            return Ancestors(self).Prepend(self);
        }

    }

}
