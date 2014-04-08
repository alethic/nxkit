using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides interfaces decorated with the <see cref="XElementInterfaceAttribute"/>.
    /// </summary>
    [Export(typeof(INodeInterfaceProvider))]
    public class ElementInterfaceProvider :
        NodeInterfaceProviderBase
    {

        static readonly List<Tuple<Tuple<string, string>, List<Type>>> map;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ElementInterfaceProvider()
        {
            // applicable types to search for attributes
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(i => i.GetTypes())
                .Where(i => i.IsClass && !i.IsAbstract)
                .ToList();

            // attributes decorating types
            var attrs = types
                .Select(i => new { Type = i, Attributes = i.GetCustomAttributes<NXElementInterfaceAttribute>() })
                .Where(i => i.Attributes.Any())
                .ToList();

            // pairs of namespace/localname associated to type
            var pairs = attrs
                .SelectMany(i => i.Attributes.Select(j => new { Type = i.Type, Attribute = j }))
                .Select(i => new { Key = Tuple.Create(i.Attribute.NamespaceName, i.Attribute.LocalName), Type = i.Type })
                .ToList();

            // group by unique namespace/localname pairs
            var group = pairs
                .GroupBy(i => i.Key)
                .Select(i => Tuple.Create(i.Key, i.Select(j => j.Type).ToList()));

            // finish map
            map = group.ToList();
        }

        /// <summary>
        /// Tests whether the given <see cref="XName"/> matches one or both of the specified values.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        /// <returns></returns>
        bool Predicate(XName test, string namespaceName, string localName)
        {
            if (namespaceName != null &&
                namespaceName != test.NamespaceName)
                return false;

            if (localName != null &&
                localName != test.LocalName)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the interfaces for the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override IEnumerable<object> GetInterfaces(XNode node)
        {
            var element = node as XElement;
            if (element == null)
                yield break;

            // all types which are available
            var types = map
                .Where(i => Predicate(element.Name, i.Item1.Item1, i.Item1.Item2))
                .SelectMany(i => i.Item2)
                .ToList();

            foreach (var instance in GetInstances(element, types))
                yield return instance;
        }

        /// <summary>
        /// Obtains the list of interfaces.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<object> GetInstances(XElement element, IEnumerable<Type> types)
        {
            var objects = types
                .Select(i => GetOrCreate(element, i, () => CreateInstance(element, i)))
                .Where(i => i != null)
                .ToList();

            return objects;
        }

        /// <summary>
        /// Creates hte specified instance type.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateInstance(XElement element, Type type)
        {
            var ctor = type.GetConstructor(new[] { typeof(XElement) });
            if (ctor == null)
                throw new NullReferenceException("Could not find ctor accepting XElement.");

            // create new instance
            return ctor.Invoke(new object[] { element });
        }

    }

}
