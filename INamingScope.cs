namespace NXKit
{

    /// <summary>
    /// Indicates that a <see cref="Visual"/> provides a scope under which unique IDs must be generated.
    /// </summary>
    public interface INamingScope
    {

        /// <summary>
        /// Gets the unique identifier that establishes the scope.
        /// </summary>
        string UniqueId { get; }

    }

}
