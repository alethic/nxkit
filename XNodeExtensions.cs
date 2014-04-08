using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides extension methods for <see cref="XNode"/> instances.
    /// </summary>
    public static class XNodeExtensions
    {

        /// <summary>
        /// Gets all the implemented interfaces of the given <see cref="XNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return node.Host().Container
                .GetExportedValues<INodeInterfaceProvider>()
                .SelectMany(i => i.GetInterfaces(node));
        }

        /// <summary>
        /// Gets the implemented interfaces of the given <see cref="XNode"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XNode node, Type type)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).Where(i => type.IsInstanceOfType(i));
        }

        /// <summary>
        /// Gets the implemented interfaces of the given <see cref="XNode"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> Interfaces<T>(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).OfType<T>();
        }

        /// <summary>
        /// Gets the specific interface of the given <see cref="XNode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T InterfaceOrDefault<T>(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces<T>(node).FirstOrDefault();
        }

        /// <summary>
        /// Gets the specific interface of the given <see cref="XNode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Interface<T>(this XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Ensures(Contract.Result<T>() != null);

            var i = InterfaceOrDefault<T>(node);
            if (i == null)
                throw new NullReferenceException();

            return i;
        }

    }

}
