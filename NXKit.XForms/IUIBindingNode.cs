namespace NXKit.XForms
{

    /// <summary>
    /// Describes a form element that participates as a UI binding element.
    /// </summary>
    [Public]
    public interface IUIBindingNode
    {

        /// <summary>
        /// Gets the UI binding for the node.
        /// </summary>
        UIBinding UIBinding { get; }

    }

}
