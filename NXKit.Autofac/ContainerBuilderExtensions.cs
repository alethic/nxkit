using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Lifetime;
using Autofac.Features.OpenGenerics;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit.Autofac
{

    /// <summary>
    /// Provides extensions for registering NXKit assemblies into Autofac builders.
    /// </summary>
    public static class ContainerBuilderExtensions
    {

        /// <summary>
        /// Registers the specified NXKit assembly into the given Autofac builder.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterNXKitAssemblies(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
                builder.RegisterNXKitAssembly(assembly);

            return builder;
        }

        /// <summary>
        /// Registers the specified NXKit assembly into the given Autofac builder.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterNXKitAssembly(this ContainerBuilder builder, Assembly assembly)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            // makes the composition context wrapper available to NXKit
            builder.Register(ctx => new CompositionContext(ctx.Resolve<ILifetimeScope>()))
                .As<ICompositionContext>()
                .IfNotRegistered(typeof(ICompositionContext));

            RegisterExports(builder, assembly, CompositionScope.Global);
            RegisterExports(builder, assembly, CompositionScope.Host);
            RegisterExports(builder, assembly, CompositionScope.Object);
            RegisterExports(builder, assembly, CompositionScope.Transient);

            return builder;
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
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        /// <param name="scope"></param>
        static void RegisterExports(ContainerBuilder builder, Assembly assembly, CompositionScope scope)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            foreach (var type in assembly.GetTypes())
                RegisterExports(builder, type, scope);
        }

        /// <summary>
        /// Registers a specific exported type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        static void RegisterExports(ContainerBuilder builder, Type type, CompositionScope scope)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var exports = GetCustomAttributesThatImplement<ExportAttribute>(type).Where(i => i.Scope == scope).ToArray();
            if (exports.Length > 0)
                RegisterExports(builder, type, exports, scope);
        }

        /// <summary>
        /// Registers a specific exported type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="exports"></param>
        /// <param name="scope"></param>
        static void RegisterExports(ContainerBuilder builder, Type type, ExportAttribute[] exports, CompositionScope scope)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (exports is null)
                throw new ArgumentNullException(nameof(exports));
            if (exports.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(exports));

            // generic type exports are specially handled
            if (type.IsGenericTypeDefinition)
            {
                RegisterGenericExports(builder, type, exports, scope);
                return;
            }

            // make underlying types available
            SetScope(builder.RegisterType(type).AsSelf(), scope);

            // make each export attribute available
            foreach (var export in exports)
                RegisterExport(builder, type, export, scope);
        }

        /// <summary>
        /// Registers a specified export for a type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="export"></param>
        /// <param name="scope"></param>
        static void RegisterExport(ContainerBuilder builder, Type type, ExportAttribute export, CompositionScope scope)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (export is null)
                throw new ArgumentNullException(nameof(export));

            var n = type.Name;

            // types to export
            var exportTypes = export.Type != null ? new[] { export.Type } : type.GetInterfaces().Concat(type.Recurse(i => i.BaseType)).Distinct();
            exportTypes = exportTypes.Where(i => i != typeof(object));

            // set of types to export as
            foreach (var exportTypeIter in exportTypes)
            {
                var exportType = exportTypeIter;
                if (exportType != type)
                    SetScope(builder.Register(ctx => ctx.Resolve(type)).As(exportType).ExternallyOwned(), scope);

                // make available as raw interface type
                var wrapperType = typeof(IExport<>).MakeGenericType(exportType);
                var makeExportMethodInfo = MakeExportMethodInfo.MakeGenericMethod(exportType);
                SetScope(builder.Register(ctx => makeExportMethodInfo.Invoke(null, new object[] { ctx, type })).As(wrapperType).ExternallyOwned(), scope);

                // make available as metadata types
                foreach (var metadataTypeIter in export.GetType().GetInterfaces().Where(i => i.IsAssignableTo<IExportMetadata>()))
                {
                    var metadataType = metadataTypeIter;
                    var metadataWrapperType = typeof(IExport<,>).MakeGenericType(exportType, metadataType);
                    var makeMetadataExportMethodInfo = MakeMetadataExportMethodInfo.MakeGenericMethod(exportType, metadataType);
                    SetScope(builder.Register(ctx => makeMetadataExportMethodInfo.Invoke(null, new object[] { ctx, type, export })).As(metadataWrapperType).ExternallyOwned(), scope);
                }
            }
        }

        static readonly MethodInfo MakeExportMethodInfo = typeof(ContainerBuilderExtensions).GetMethod(nameof(MakeExport), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Makes an <see cref="IExport{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static IExport<TValue> MakeExport<TValue>(IComponentContext context, Type type)
        {
            var ctx = context.Resolve<IComponentContext>();
            return new Export<TValue>(() => (TValue)ctx.Resolve(type));
        }

        static readonly MethodInfo MakeMetadataExportMethodInfo = typeof(ContainerBuilderExtensions).GetMethod(nameof(MakeMetadataExport), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Makes an <see cref="IExport{TValue, TMetadata}"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        static IExport<TValue> MakeMetadataExport<TValue, TMetadata>(IComponentContext context, Type type, TMetadata metadata)
            where TMetadata : IExportMetadata
        {
            var ctx = context.Resolve<IComponentContext>();
            return new Export<TValue, TMetadata>(() => (TValue)ctx.Resolve(type), metadata);
        }

        /// <summary>
        /// Configures the scope of a registration.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> SetScope<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, CompositionScope scope)
        {
            switch (scope)
            {
                case CompositionScope.Global:
                    return registration.SingleInstance();
                case CompositionScope.Host:
                    return registration.InstancePerMatchingLifetimeScope("NXKit.Host");
                case CompositionScope.Object:
                    return registration.InstancePerMatchingLifetimeScope("NXKit.Object");
                case CompositionScope.Transient:
                    return registration.InstancePerDependency();
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Configures the scope of a registration.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        static string GetTag(CompositionScope scope)
        {
            switch (scope)
            {
                case CompositionScope.Global:
                    return null;
                case CompositionScope.Host:
                    return "NXKit.Host";
                case CompositionScope.Object:
                    return "NXKit.Object";
                case CompositionScope.Transient:
                    return null;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Registers an open generic exported type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="export"></param>
        /// <param name="scope"></param>
        /// <param name="tag"></param>
        static void RegisterGenericExports(ContainerBuilder builder, Type type, ExportAttribute[] exports, CompositionScope scope)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (exports is null)
                throw new ArgumentNullException(nameof(exports));
            if (exports.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(exports));

            var registrationData = new RegistrationData(new TypedService(exports[0].Type));
            registrationData.AddServices(exports.Select(i => new TypedService(i.Type)));
            registrationData.Lifetime = GetTag(scope) is string tag ? (IComponentLifetime)new MatchingScopeLifetime(tag) : new RootScopeLifetime();
            registrationData.Sharing = InstanceSharing.Shared;
            registrationData.Ownership = InstanceOwnership.OwnedByLifetimeScope;
            builder.RegisterSource(new OpenGenericRegistrationSource(registrationData, new ReflectionActivatorData(type)));
        }

    }

}
