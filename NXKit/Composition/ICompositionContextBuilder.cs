namespace NXKit.Composition
{

    /// <summary>
    /// Describes an interface supporting the addition of instances to a scope.
    /// </summary>
    public interface ICompositionContextBuilder
    {

        /// <summary>
        /// Adds an instance to the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        ICompositionContextBuilder AddInstance<T>(T instance)
            where T : class;

    }

}