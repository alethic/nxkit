namespace NXKit.XForms
{

    /// <summary>
    /// Describes a UI element.
    /// </summary>
    [Remote]
    public interface IUINode
    {

        /// <summary>
        /// Gets whether the given node is considered relevant.
        /// </summary>
        [Remote]
        bool Relevant { get; }

        /// <summary>
        /// Gets whether the given node is considered read-only.
        /// </summary>
        [Remote]
        bool ReadOnly { get; }

        /// <summary>
        /// Gets whether the given node is considered required.
        /// </summary>
        [Remote]
        bool Required { get; }

        /// <summary>
        /// Gets whether the give node is considered valid.
        /// </summary>
        [Remote]
        bool Valid { get; }

    }

}
