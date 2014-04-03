using System.Collections.Generic;

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
        public static IEnumerable<NXElement> Ancestors(this NXNode self)
        {
            var node = self.Parent;
            while (node != null)
            {
                yield return node;
                node = node.Parent;
            }
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
        /// Gets all ancestor containers of the given <see cref="NXElement"/>, including itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<NXElement> AncestorsAndSelf(this NXElement self)
        {
            return Ancestors(self).Prepend(self);
        }

    }

}
