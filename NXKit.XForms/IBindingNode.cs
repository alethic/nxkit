namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element that returns a binding.
    /// </summary>
    public interface IBindingNode
    {

        /// <summary>
        /// Gets the binding defined by the element.
        /// </summary>
        Binding Binding { get; }

    }

}
