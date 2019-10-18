using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit.Extensions.DependencyInjection
{

    /// <summary>
    /// Provides extensions for registering NXKit assemblies into Autofac builders.
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Registers the specified NXKit assembly into the given service collection.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddNXKit(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services.AddNXKit(SafeAssemblyLoader.LoadAll().ToArray());
        }

        /// <summary>
        /// Registers the specified NXKit assembly into the given service collection.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddNXKit(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
                services.AddNXKit(assembly);

            return services;
        }

        /// <summary>
        /// Registers the specified NXKit assembly into the given service collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddNXKitAssembly(this IServiceCollection services, Assembly assembly)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            // makes the composition context wrapper available to NXKit
            services.AddTransient<ICompositionContext>(ctx => new CompositionContext(ctx.GetRequiredService<IServiceScope>()));

            // add all of the various exports to the container
            AddExports(services, assembly, CompositionScope.Global);
            AddExports(services, assembly, CompositionScope.Host);
            AddExports(services, assembly, CompositionScope.Object);
            AddExports(services, assembly, CompositionScope.Transient);

            return services;
        }

        /// <summary>
        /// Gets all of the custom attributes on the type that implement the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        static IEnumerable<T> GetCustomAttributesThatImplement<T>(Type type) => type.GetCustomAttributes(typeof(T)).OfType<T>();

        /// <summary>
        /// Registers all of the exports in the <see cref="Assembly"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="scope"></param>
        static void AddExports(IServiceCollection services, Assembly assembly, CompositionScope scope)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            foreach (var type in SafeAssemblyLoader.GetTypes(assembly))
                AddExports(services, type, scope);
        }

        /// <summary>
        /// Registers a specific exported type.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        static void AddExports(IServiceCollection services, Type type, CompositionScope scope)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var exports = GetCustomAttributesThatImplement<ExportAttribute>(type).Where(i => i.Scope == scope).ToArray();
            if (exports.Length > 0)
                AddExports(services, type, exports, scope);
        }

        /// <summary>
        /// Registers a specific exported type.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="exports"></param>
        /// <param name="scope"></param>
        static void AddExports(IServiceCollection services, Type type, ExportAttribute[] exports, CompositionScope scope)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (exports is null)
                throw new ArgumentNullException(nameof(exports));
            if (exports.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(exports));

            // generic type exports are specially handled
            if (type.IsGenericTypeDefinition)
                throw new NotImplementedException();

            // make underlying types available
            AddService(services, type, type, scope);

            // make each export attribute available
            foreach (var export in exports)
                AddExport(services, type, export, scope);
        }

        /// <summary>
        /// Registers a specified export for a type.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="export"></param>
        /// <param name="scope"></param>
        static void AddExport(IServiceCollection services, Type type, ExportAttribute export, CompositionScope scope)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (export is null)
                throw new ArgumentNullException(nameof(export));

            // types to export
            var exportTypes = export.Type != null ? new[] { export.Type } : type.GetInterfaces().Concat(type.Recurse(i => i.BaseType)).Distinct();
            exportTypes = exportTypes.Where(i => i != typeof(object));

            // set of types to export as
            foreach (var exportTypeIter in exportTypes)
            {
                var exportType = exportTypeIter;
                if (exportType != type)
                    AddService(services, exportType, ctx => ctx.GetRequiredService(type), scope);

                // make available as raw interface type
                var wrapperType = typeof(IExport<>).MakeGenericType(exportType);
                var makeExportMethodInfo = MakeExportMethodInfo.MakeGenericMethod(exportType);
                AddService(services, wrapperType, ctx => makeExportMethodInfo.Invoke(null, new object[] { ctx, type }), scope);

                // make available as metadata types
                foreach (var metadataTypeIter in export.GetType().GetInterfaces().Where(i => typeof(IExportMetadata).IsAssignableFrom(i)))
                {
                    var metadataType = metadataTypeIter;
                    var metadataWrapperType = typeof(IExport<,>).MakeGenericType(exportType, metadataType);
                    var makeMetadataExportMethodInfo = MakeMetadataExportMethodInfo.MakeGenericMethod(exportType, metadataType);
                    AddService(services, metadataWrapperType, ctx => makeMetadataExportMethodInfo.Invoke(null, new object[] { ctx, type, export }), scope);
                }
            }
        }

        static readonly MethodInfo MakeExportMethodInfo = typeof(ServiceCollectionExtensions).GetMethod(nameof(MakeExport), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Makes an <see cref="IExport{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static IExport<TValue> MakeExport<TValue>(IServiceProvider provider, Type type)
        {
            var ctx = provider.GetRequiredService<IServiceProvider>();
            return new Export<TValue>(() => (TValue)ctx.GetRequiredService(type));
        }

        static readonly MethodInfo MakeMetadataExportMethodInfo = typeof(ServiceCollectionExtensions).GetMethod(nameof(MakeMetadataExport), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Makes an <see cref="IExport{TValue, TMetadata}"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        static IExport<TValue> MakeMetadataExport<TValue, TMetadata>(IServiceProvider provider, Type type, TMetadata metadata)
            where TMetadata : IExportMetadata
        {
            var ctx = provider.GetRequiredService<IServiceProvider>();
            return new Export<TValue, TMetadata>(() => (TValue)ctx.GetRequiredService(type), metadata);
        }

        /// <summary>
        /// Adds a service to the collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        static IServiceCollection AddService(IServiceCollection services, Type serviceType, Type implementationType, CompositionScope scope)
        {
            switch (scope)
            {
                case CompositionScope.Global:
                    return services.AddSingleton(serviceType, implementationType);
                case CompositionScope.Host:
                    return services.AddScoped(serviceType, implementationType);
                case CompositionScope.Object:
                    return services.AddScoped(serviceType, implementationType);
                case CompositionScope.Transient:
                    return services.AddTransient(serviceType, implementationType);
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Adds a service to the collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationFactory"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        static IServiceCollection AddService(IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory, CompositionScope scope)
        {
            switch (scope)
            {
                case CompositionScope.Global:
                    return services.AddSingleton(serviceType, implementationFactory);
                case CompositionScope.Host:
                    return services.AddScoped(serviceType, implementationFactory);
                case CompositionScope.Object:
                    return services.AddScoped(serviceType, implementationFactory);
                case CompositionScope.Transient:
                    return services.AddTransient(serviceType, implementationFactory);
                default:
                    throw new InvalidOperationException();
            }
        }

    }

}
