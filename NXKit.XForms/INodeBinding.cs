namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element that returns a binding.
    /// </summary>
    public interface INodeBinding
    {

        /// <summary>
        /// Gets the binding defined by the element.
        /// </summary>
        Binding Binding { get; }

    }

}
