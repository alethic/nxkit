using System.Collections.Generic;
using System.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="NXObject"/> instances.
    /// </summary>
    public static class NXObjectExtensions
    {

        /// <summary>
        /// Gets all the implemented interfaces of the given <see cref="NXObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this NXObject obj)
        {
            return obj.Document.Modules()
                .SelectMany(i => i.GetInterfaces(obj));
        }

        /// <summary>
        /// Gets the specific interface of the given <see cref="NXObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Interface<T>(this NXObject obj)
        {
            return Interfaces(obj)
                .OfType<T>()
                .FirstOrDefault();
        }

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

    }

}
