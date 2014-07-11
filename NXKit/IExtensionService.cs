namespace NXKit
{

    /// <summary>
    /// Gets the extension for the given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtensionService<T>
    {

        /// <summary>
        /// Gets the value of the extension.
        /// </summary>
        T Value { get; }

    }

}
