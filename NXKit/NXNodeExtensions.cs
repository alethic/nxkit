using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides extension methods for <see cref="NXNode"/> instances.
    /// </summary>
    public static class NXNodeExtensions
    {


        /// <summary>
        /// Gets all the implemented interfaces of the given <see cref="NXNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(node.Document != null);
            Contract.Requires<ArgumentNullException>(node.Document.Container != null);

            return node.Document.Container
                .GetExportedValues<INodeInterfaceProvider>()
                .SelectMany(i => i.GetInterfaces(node));
        }

        /// <summary>
        /// Gets the implemented interfaces of the given <see cref="NXNode"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this NXNode node, Type type)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).Where(i => type.IsInstanceOfType(i));
        }

        /// <summary>
        /// Gets the implemented interfaces of the given <see cref="NXNode"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> Interfaces<T>(this NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).OfType<T>();
        }

        /// <summary>
        /// Gets the specific interface of the given <see cref="NXNode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T InterfaceOrDefault<T>(this NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces<T>(node).FirstOrDefault();
        }

        /// <summary>
        /// Gets the specific interface of the given <see cref="NXNode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Interface<T>(this NXNode node)
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
