using System;

using Autofac;

using NXKit.Composition;

namespace NXKit.Autofac
{

    /// <summary>
    /// Autofac based implementation of <see cref="ICompositionContext"/>.
    /// </summary>
    class CompositionContext : ICompositionContext
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="scope"></param>
        public CompositionContext(ILifetimeScope scope)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public ICompositionContext BeginContext(CompositionScope scope)
        {
            return new CompositionContext(this.scope.BeginLifetimeScope(CompositionScopeToTag(scope)));
        }

        public ICompositionContext BeginContext(CompositionScope scope, Action<ICompositionContextBuilder> builder)
        {
            return new CompositionContext(this.scope.BeginLifetimeScope(CompositionScopeToTag(scope), b => builder(new CompositionContextBuilder(b))));
        }

        string CompositionScopeToTag(CompositionScope scope)
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
                default:
                    throw new InvalidOperationException("Invalid scope for tagging.");
            }
        }

        public T Resolve<T>()
        {
            return scope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return scope.Resolve(type);
        }

        public void Dispose()
        {
            scope.Dispose();
        }

    }

}
