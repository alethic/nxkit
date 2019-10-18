using Microsoft.Extensions.DependencyInjection;

using NXKit.Composition;

using System;

namespace NXKit.Extensions.DependencyInjection
{

    /// <summary>
    /// Implements the <see cref="ICompositionContextBuilder"/> interface for Microsoft Dependency Injection scopes.
    /// </summary>
    class CompositionContextBuilder : ICompositionContextBuilder
    {

        readonly IServiceCollection services;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="builder"></param>
        public CompositionContextBuilder(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public ICompositionContextBuilder AddInstance<T>(T instance)
            where T : class
        {
            throw new NotSupportedException();
        }

    }

}
