using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NXKit.Xml;

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

            var func = GetConstructor(obj, type, obj.Host().Container);
            if (func == null)
                throw new InvalidOperationException("Could not find ctor for interface type.");

            return func();
        }

        /// <summary>
        /// Gets the most appropriate constructor that can be fulfilled from the container.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        Func<object> GetConstructor(XObject obj, Type type, CompositionContainer container)
        {
            return type.GetConstructors()
                .Select(i => GetConstructorFunc(obj, type, i, container))
                .Where(i => i != null)
                .OrderByDescending(i => i.Item2)
                .Select(i => i.Item1)
                .FirstOrDefault();
        }

        /// <summary>
        /// For a given constructor, generates a function that if invoked will return the new object. Returns 
        /// <c>null</c> if the given constructor cannot be fulfilled. Tuple also contains a value to sort by given the
        /// number of fulfilled parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        Tuple<Func<object>, int> GetConstructorFunc(XObject obj, Type type, ConstructorInfo ctor, CompositionContainer container)
        {
            var p = ctor.GetParameters();
            var l = new object[p.Length];
            int c = 0;

            for (int i = 0; i < l.Length; i++)
            {
                if (p[i].ParameterType.IsInstanceOfType(obj))
                {
                    l[i] = obj;

                    // increment filled parameter count
                    c++;
                }
                else
                {
                    l[i] = GetConstructorParameterValue(obj, type, ctor, p[i], container);

                    // increment filled parameter count
                    if (l[i] != null)
                        c++;
                }
            }

            return Tuple.Create<Func<object>, int>(() => ctor.Invoke(l), c);
        }

        /// <summary>
        /// Gets a value for the given constructor parameter from the container.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        /// <param name="param"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        object GetConstructorParameterValue(XObject obj, Type type, ConstructorInfo ctor, ParameterInfo param, CompositionContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(ctor != null);
            Contract.Requires<ArgumentNullException>(param != null);
            Contract.Requires<ArgumentNullException>(container != null);

            var paramContractType = GetContractType(param.ParameterType);
            var paramContractName = GetContractName(paramContractType);
            var paramTypeIdentity = AttributedModelServices.GetTypeIdentity(paramContractType);

            // ImportAttribute present
            var attr1 = param.GetCustomAttribute<ImportAttribute>();
            if (attr1 != null)
                return container.GetExports(ReflectionModelServices.CreateImportDefinition(
                        new Lazy<ParameterInfo>(() => param),
                        attr1.ContractName ?? paramContractName,
                        attr1.ContractType != null ? AttributedModelServices.GetTypeIdentity(attr1.ContractType) : paramTypeIdentity,
                        Enumerable.Empty<KeyValuePair<string, Type>>(),
                        ImportCardinality.ZeroOrMore,
                        CreationPolicy.Any,
                        null))
                    .Select(i => i.Value)
                    .FirstOrDefault();

            // ImportManyAttribute present
            var attr2 = param.GetCustomAttribute<ImportManyAttribute>();
            if (attr2 != null)
                return container.GetExports(ReflectionModelServices.CreateImportDefinition(
                    new Lazy<ParameterInfo>(() => param),
                        attr2.ContractName ?? paramContractName,
                        attr2.ContractType != null ? AttributedModelServices.GetTypeIdentity(attr2.ContractType) : paramTypeIdentity,
                        Enumerable.Empty<KeyValuePair<string, Type>>(),
                        ImportCardinality.ZeroOrMore,
                        CreationPolicy.Any,
                        null))
                    .Select(i => i.Value);

            // no attribute present
            if (typeof(IEnumerable).IsAssignableFrom(param.ParameterType))
                return container.GetExports(ReflectionModelServices.CreateImportDefinition(
                    new Lazy<ParameterInfo>(() => param),
                        paramContractName,
                        paramTypeIdentity,
                        Enumerable.Empty<KeyValuePair<string, Type>>(),
                        ImportCardinality.ZeroOrMore,
                        CreationPolicy.Any,
                        null))
                    .Select(i => i.Value);

            return container.GetExports(ReflectionModelServices.CreateImportDefinition(
                new Lazy<ParameterInfo>(() => param),
                    paramContractName,
                    paramTypeIdentity,
                    Enumerable.Empty<KeyValuePair<string, Type>>(),
                    ImportCardinality.ZeroOrMore,
                    CreationPolicy.Any,
                    null))
                .Select(i => i.Value)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the contract name of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetContractName(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return AttributedModelServices.GetContractName(type);
        }

        /// <summary>
        /// Gets the contract type of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetContractType(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            return typeof(IEnumerable).IsAssignableFrom(type) ? GetCollectionContractType(type) : type;
        }

        /// <summary>
        /// Gets the contract type of the given collection type.
        /// </summary>
        /// <param name="collectionType"></param>
        /// <returns></returns>
        Type GetCollectionContractType(Type collectionType)
        {
            Contract.Requires<ArgumentNullException>(collectionType != null);
            Contract.Requires<ArgumentException>(typeof(IEnumerable).IsAssignableFrom(collectionType));
            Contract.Ensures(Contract.Result<Type>() != null);

            if (!collectionType.IsGenericType)
                return typeof(object);
            else
                return collectionType
                    .GetGenericArguments()
                    .First();
        }

    }

}
