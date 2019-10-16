using Autofac;

using NXKit.Composition;

namespace NXKit.Autofac
{

    /// <summary>
    /// Implements the <see cref="ICompositionContextBuilder"/> interface for Autofac scopes.
    /// </summary>
    class CompositionContextBuilder : ICompositionContextBuilder
    {

        readonly ContainerBuilder builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="builder"></param>
        public CompositionContextBuilder(ContainerBuilder builder)
        {
            this.builder = builder;
        }

        public ICompositionContextBuilder AddInstance<T>(T instance)
            where T : class
        {
            builder.RegisterInstance<T>(instance);
            return this;
        }

    }

}