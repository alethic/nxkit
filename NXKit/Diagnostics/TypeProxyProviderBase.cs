namespace NXKit.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="ITypeProxyProvider"/> base implementation that supports a single generic type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TypeProxyProviderBase<T> :
        ITypeProxyProvider
    {

        public object Proxy(object input)
        {
            return Proxy((T)input);
        }

        /// <summary>
        /// Provides a proxy object for the given type.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract object Proxy(T input);

    }

}
