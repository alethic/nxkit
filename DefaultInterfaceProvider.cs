using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides interfaces decorated with the <see cref="XElementInterfaceAttribute"/>.
    /// </summary>
    [Export(typeof(INodeInterfaceProvider))]
    public class DefaultInterfaceProvider :
        NodeInterfaceProviderBase
    {

        static readonly List<Tuple<Tuple<XmlNodeType, string, string>, List<Type>>> map;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static DefaultInterfaceProvider()
        {
            // applicable types to search for attributes
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(i => i.GetTypes())
                .Where(i => i.IsClass && !i.IsAbstract)
                .ToList();

            // attributes decorating types
            var attrs = types
                .Select(i => new { Type = i, Attributes = i.GetCustomAttributes<InterfaceAttribute>() })
                .Where(i => i.Attributes.Any())
                .ToList();

            // pairs of namespace/localname associated to type
            var pairs = attrs
                .SelectMany(i => i.Attributes.Select(j => new { Type = i.Type, Attribute = j }))
                .Select(i => new { Key = Tuple.Create(i.Attribute.NodeType, i.Attribute.NamespaceName, i.Attribute.LocalName), Type = i.Type })
                .ToList();

            // group by unique namespace/localname pairs
            var group = pairs
                .GroupBy(i => i.Key)
                .Select(i => Tuple.Create(i.Key, i.Select(j => j.Type).ToList()));

            // finish map
            map = group.ToList();
        }

        /// <summary>
        /// Tests whether the given <see cref="XObject"/> matches the filter.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nodeType"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        /// <returns></returns>
        bool Predicate(XObject obj, XmlNodeType nodeType, string namespaceName, string localName)
        {
            if (nodeType != XmlNodeType.None &&
                nodeType != obj.NodeType)
                return false;

            var element = obj as XElement;
            if (element != null && !Predicate(element, nodeType, namespaceName, localName))
                return false;

            return true;
        }

        /// <summary>
        /// Tests whether the given <see cref="XObjectXElement"/> matches the filter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nodeType"></param>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        /// <returns></returns>
        bool Predicate(XElement element, XmlNodeType nodeType, string namespaceName, string localName)
        {
            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
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
            // all types which are available
            var types = map
                .Where(i => Predicate(node, i.Item1.Item1, i.Item1.Item2, i.Item1.Item3))
                .SelectMany(i => i.Item2)
                .ToList();

            foreach (var instance in GetInstances(node, types))
                yield return instance;
        }

        /// <summary>
        /// Obtains the list of interfaces.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<object> GetInstances(XNode node, IEnumerable<Type> types)
        {
            var objects = types
                .Select(i => GetOrCreate(node, i, () => CreateInstance(node, i)))
                .Where(i => i != null)
                .ToList();

            return objects;
        }

        /// <summary>
        /// Creates hte specified instance type.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateInstance(XNode node, Type type)
        {
            var ctor = type.GetConstructors()
                .Where(i => i.GetParameters().Length == 1)
                .Where(i => i.GetParameters()[0].ParameterType.IsInstanceOfType(node))
                .FirstOrDefault();
            if (ctor == null)
                throw new NullReferenceException("Could not find ctor accepting XElement.");

            // create new instance
            return ctor.Invoke(new object[] { node });
        }

    }

}
