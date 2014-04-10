using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides interfaces decorated with the <see cref="InterfaceAttribute"/>.
    /// </summary>
    [Export(typeof(IInterfaceProvider))]
    public class DefaultInterfaceProvider :
        InterfaceProviderBase
    {

        static readonly List<InterfaceDescriptor> defaultDescriptors;

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

            // generate interface descriptors
            var descriptors = attrs
                .SelectMany(i => i.Attributes
                    .Select(j => new InterfaceDescriptor(j.NodeType, j.NamespaceName, j.LocalName, j.PredicateType, i.Type)))
                .ToList();

            // finish map
            defaultDescriptors = descriptors;
        }

        readonly IEnumerable<IInterfacePredicate> predicates;
        readonly List<InterfaceDescriptor> descriptors;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public DefaultInterfaceProvider(
            [ImportMany] IEnumerable<IInterfacePredicate> predicates)
            : this(predicates, defaultDescriptors)
        {
            Contract.Requires<ArgumentNullException>(predicates != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptors"></param>
        public DefaultInterfaceProvider(
            IEnumerable<IInterfacePredicate> predicates,
            List<InterfaceDescriptor> descriptors)
        {
            Contract.Requires<ArgumentNullException>(predicates != null);
            Contract.Requires<ArgumentNullException>(descriptors != null);

            this.predicates = predicates;
            this.descriptors = descriptors;
        }

        /// <summary>
        /// Gets the interfaces for the specified <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override IEnumerable<object> GetInterfaces(XObject obj)
        {
            // available interface types for the object
            var types = descriptors
                .Where(i => i.IsMatch(predicates, obj))
                .Select(i => i.Type);

            foreach (var instance in GetInstances(obj, types))
                yield return instance;
        }

        /// <summary>
        /// Obtains the list of interfaces.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<object> GetInstances(XObject obj, IEnumerable<Type> types)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(types != null);

            var objects = types
                .Select(i => GetOrCreate(obj, i, () => CreateInstance(obj, i)))
                .Where(i => i != null)
                .ToList();

            return objects;
        }

        /// <summary>
        /// Creates hte specified instance type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateInstance(XObject obj, Type type)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);

            var ctor1 = type.GetConstructors()
                .Where(i => i.GetParameters().Length == 1)
                .Where(i => i.GetParameters()[0].ParameterType.IsInstanceOfType(obj))
                .FirstOrDefault();
            if (ctor1 != null)
                return ctor1.Invoke(new object[] { obj });

            var ctor2 = type.GetConstructors()
                .Where(i => i.GetParameters().Length == 0)
                .FirstOrDefault();
            if (ctor2 != null)
                return ctor2.Invoke(new object[] { });

            throw new InvalidOperationException("Could not find ctor for interface type.");
        }

    }

}
