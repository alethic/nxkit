using System;

using Microsoft.Extensions.DependencyInjection;

using NXKit.Composition;

namespace NXKit.Extensions.DependencyInjection
{

    /// <summary>
    /// Microsoft Dependency Injection based implementation of <see cref="ICompositionContext"/>.
    /// </summary>
    class CompositionContext : ICompositionContext
    {

        readonly IServiceProvider provider;
        readonly IServiceScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="scope"></param>
        public CompositionContext(IServiceProvider provider, IServiceScope scope)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="scope"></param>
        public CompositionContext(IServiceScope scope) :
            this(scope.ServiceProvider, scope)
        {

        }

        public ICompositionContext BeginContext(CompositionScope scope)
        {
            return new CompositionContext(provider.GetRequiredService<IServiceScopeFactory>().CreateScope());
        }

        public ICompositionContext BeginContext(CompositionScope scope, Action<ICompositionContextBuilder> builder)
        {
            throw new NotSupportedException();
        }

        public T Resolve<T>()
        {
            return provider.GetRequiredService<T>();
        }

        public object Resolve(Type type)
        {
            return provider.GetRequiredService(type);
        }

        public void Dispose()
        {
            if (scope != null)
                scope.Dispose();
        }

    }

}
