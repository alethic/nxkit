namespace NXKit.XForms
{

    /// <summary>
    /// Describes a form element that participates as a UI binding element.
    /// </summary>
    [Remote]
    public interface IUIBindingNode
    {

        /// <summary>
        /// Gets the UI binding for the node.
        /// </summary>
        [Remote]
        IUIBinding UIBinding { get; }

    }

}
