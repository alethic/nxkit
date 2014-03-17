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

        /// <summary>
        /// Generates a new UniqueID within the scope.
        /// </summary>
        string GenerateUniqueId(string id);

        /// <summary>
        /// Allocates a new ID within the scope.
        /// </summary>
        /// <returns></returns>
        string AllocateId();

    }

}
