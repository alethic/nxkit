namespace NXKit.Composition
{

    /// <summary>
    /// Describes the various scopes of exports.
    /// </summary>
    public enum Scope
    {

        /// <summary>
        /// Export is derived from the provided catalog or export provider, and is not further scoped.
        /// </summary>
        Global,

        /// <summary>
        /// Export is allocated within the host container. These exports are not available to Global exports.
        /// </summary>
        Host,

        /// <summary>
        /// Export is allocated within the node container. These exports are not available to Host or Global exports.
        /// </summary>
        Node,

    }

}
