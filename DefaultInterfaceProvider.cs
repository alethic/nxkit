using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit
{

    /// <summary>
    /// Provides interfaces decorated with the <see cref="InterfaceAttribute"/>.
    /// </summary>
    [Export(typeof(IInterfaceProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DefaultInterfaceProvider :
        InterfaceProviderBase
    {

        class DescriptorTypeList :
            List<Type>
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="types"></param>
            public DescriptorTypeList(IEnumerable<Type> types)
                : base(types)
            {
                Contract.Requires<ArgumentNullException>(types != null);
            }

        }

        static readonly List<InterfaceDescriptor> defaultDescriptors;
        static readonly MethodInfo castMethodInfo = typeof(DefaultInterfaceProvider).GetMethod("CastEnumerableGeneric", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Converts the source enumerable into a generic output.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static IEnumerable CastEnumerable(IEnumerable source, Type type)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(type != null);

            return (IEnumerable)castMethodInfo.MakeGenericMethod(type)
                .Invoke(null, new object[] { source });
        }

        /// <summary>
        /// Implementation method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        static IEnumerable<T> CastEnumerableGeneric<T>(IEnumerable source)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            return source.Cast<T>();
        }

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

        readonly IHostContainer container;
        readonly IEnumerable<IInterfacePredicate> predicates;
        readonly List<InterfaceDescriptor> descriptors;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicates"></param>
        [ImportingConstructor]
        public DefaultInterfaceProvider(
            IHostContainer container,
            [ImportMany] IEnumerable<IInterfacePredicate> predicates)
            : this(container, predicates, defaultDescriptors)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(predicates != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="catalog"></param>
        /// <param name="predicates"></param>
        /// <param name="descriptors"></param>
        public DefaultInterfaceProvider(
            IHostContainer container,
            IEnumerable<IInterfacePredicate> predicates,
            List<InterfaceDescriptor> descriptors)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(predicates != null);
            Contract.Requires<ArgumentNullException>(descriptors != null);

            this.container = container;
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
            var types = obj.AnnotationOrCreate<DescriptorTypeList>(() =>
                new DescriptorTypeList(descriptors
                    .Where(i => i.IsMatch(predicates, obj))
                    .Select(i => i.Type)));

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
        /// Creates the specified instance type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateInstance(XObject obj, Type type)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);

            var func = GetConstructor(obj, type, obj.Container());
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
        Func<object> GetConstructor(XObject obj, Type type, IContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(container != null);

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
        Tuple<Func<object>, int> GetConstructorFunc(XObject obj, Type type, ConstructorInfo ctor, IContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(ctor != null);
            Contract.Requires<ArgumentNullException>(container != null);

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
        /// Builds a typed <see cref="Delegate"/> for obtaining the given interface type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        Func<T> BuildDelegate<T>(XObject obj, IContainer container)
        {
            return () => (T)GetConstructorParameterValue(obj, typeof(T), container);
        }

        /// <summary>
        /// The <see cref="MethodInfo"/> for the generic BuildDelegate method.
        /// </summary>
        MethodInfo BuildDelegateMethodInfo = typeof(DefaultInterfaceProvider).GetMethod(
            "BuildDelegate",
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { typeof(XObject), typeof(IContainer) },
            null);

        /// <summary>
        /// Gets a value for the given constructor parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        /// <param name="param"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        object GetConstructorParameterValue(XObject obj, Type type, ConstructorInfo ctor, ParameterInfo param, IContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(ctor != null);
            Contract.Requires<ArgumentNullException>(param != null);
            Contract.Requires<ArgumentNullException>(container != null);

            // handle Lazy<T> references
            if (param.ParameterType.IsGenericType &&
                param.ParameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
            {
                var interfaceType = param.ParameterType.GetGenericArguments()[0];
                if (interfaceType == null)
                    return null;

                // generate function that will query interfaces on demand
                var func = BuildDelegateMethodInfo.MakeGenericMethod(interfaceType)
                    .Invoke(this, new object[] { obj, container });

                // generate new Lazy<T>
                return Activator.CreateInstance(param.ParameterType, new object[] { func });
            }

            // handle Func<T> references
            if (param.ParameterType.IsGenericType &&
                param.ParameterType.GetGenericTypeDefinition() == typeof(Func<>))
            {
                var interfaceType = param.ParameterType.GetGenericArguments()[0];
                if (interfaceType == null)
                    return null;

                // generate function that will query interfaces on demand
                return BuildDelegateMethodInfo.MakeGenericMethod(interfaceType)
                    .Invoke(this, new object[] { obj, container });
            }

            // by default support only the container
            return GetConstructorParameterValueFromContainer(obj, param, container);
        }

        /// <summary>
        /// Helper method for lazy constructor parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        object GetConstructorParameterValue(XObject obj, Type type, IContainer container)
        {
            return
                GetConstructorParameterValueFromInterface(obj, type, container) ??
                GetConstructorParameterValueFromContainer(obj, type, container);
        }

        /// <summary>
        /// Helper method for lazy constructor parameter from interface.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetConstructorParameterValueFromInterface(XObject obj, Type type, IContainer container)
        {
            return obj
                .Interfaces(type)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a value for the given constructor parameter from the container.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        object GetConstructorParameterValueFromContainer(XObject obj, ParameterInfo param, IContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(param != null);
            Contract.Requires<ArgumentNullException>(container != null);

            return GetConstructorParameterValueFromContainer(
                obj,
                param.ParameterType,
                container);
        }

        /// <summary>
        /// Gets a value for the given constructor parameter from the container.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        object GetConstructorParameterValueFromContainer(XObject obj, Type type, IContainer container)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(container != null);

            var paramContractType = GetContractType(type);
            var paramContractName = GetContractName(paramContractType);
            var paramTypeIdentity = AttributedModelServices.GetTypeIdentity(paramContractType);

            // resolve all available instances
            var instances = container.GetExports(new ContractBasedImportDefinition(
                    paramContractName,
                    paramTypeIdentity,
                    Enumerable.Empty<KeyValuePair<string, Type>>(),
                    ImportCardinality.ZeroOrMore,
                    false,
                    false,
                    CreationPolicy.Any,
                    null))
                .Select(i => i.Value);

            // return type appropriate for interface
            return typeof(IEnumerable).IsAssignableFrom(type) ?
                CastEnumerable(instances, paramContractType) :
                instances.FirstOrDefault();
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
