using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Provides interfaces decorated with the <see cref="NXElementAttribute"/>.
    /// </summary>
    [Export(typeof(INodeInterfaceProvider))]
    public class ElementInterfaceProvider :
        NodeInterfaceProviderBase
    {

        static readonly Dictionary<XName, List<Type>> map = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(i => i.GetTypes())
            .Where(i => i.IsClass && !i.IsAbstract)
            .Select(i => new { Type = i, Attribute = i.GetCustomAttribute<NXElementAttribute>() })
            .Where(i => i.Attribute != null)
            .Select(i => new { Type = i.Type, XName = i.Attribute.Name })
            .GroupBy(i => i.XName)
            .Select(i => new { XName = i.Key, Types = i.Select(j => j.Type).ToList() })
            .ToDictionary(i => i.XName, i => i.Types);

        /// <summary>
        /// Gets the interfaces for the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override IEnumerable<object> GetInterfaces(NXNode node)
        {
            var element = node as NXElement;
            if (element == null)
                yield break;

            var types = map.GetOrDefault(element.Name);
            if (types == null)
                yield break;

            // generate instances and return
            foreach (var instance in GetInstances(element, types))
                yield return instance;
        }

        /// <summary>
        /// Obtains the list of interfaces.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<object> GetInstances(NXElement element, IEnumerable<Type> types)
        {
            return types
                .Select(i => GetOrCreate(element, () => CreateInstance(element, i)))
                .Where(i => i != null);
        }

        /// <summary>
        /// Creates hte specified instance type.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateInstance(NXElement element, Type type)
        {
            var ctor = type.GetConstructor(new[] { typeof(NXElement) });
            if (ctor == null)
                throw new NullReferenceException("Could not find ctor accepting NXElement.");

            // create new instance
            return ctor.Invoke(new object[] { element });
        }

    }

}
