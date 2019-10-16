namespace NXKit.Composition
{

    /// <summary>
    /// Describes the various scopes of exports.
    /// </summary>
    public enum CompositionScope
    {

        /// <summary>
        /// Export is derived from the provided catalog or export provider, and is not further scoped.
        /// </summary>
        Global = 0,

        /// <summary>
        /// Export is allocated within the host container. These exports are not available to Global exports.
        /// </summary>
        Host = 1,

        /// <summary>
        /// Export is allocated within the object container. These exports are not available to Host or Global exports.
        /// </summary>
        Object = 2,

        /// <summary>
        /// Export is available per-dependency. These exports are available all the time, and associated with the context that requests them.
        /// </summary>
        Transient = 3,

    }

}
