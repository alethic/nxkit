using System;

namespace NXKit.Composition
{

    /// <summary>
    /// Provides access to exports within a context.
    /// </summary>
    public interface ICompositionContext : IDisposable
    {

        /// <summary>
        /// Begins a new scope of the specified type.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        ICompositionContext BeginContext(CompositionScope scope);

        /// <summary>
        /// Begins a new scope of the specified type.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        ICompositionContext BeginContext(CompositionScope scope, Action<ICompositionContextBuilder> builder);

        /// <summary>
        /// Resolves an object of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves an object of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);

    }

}
