namespace NXKit.XForms
{

    /// <summary>
    /// Describes a UI element.
    /// </summary>
    [Remote]
    public interface IUINode
    {

        [Remote]
        UI UI { get; }

        /// <summary>
        /// Initiates a refresh of the node.
        /// </summary>
        void Refresh();

    }

}
