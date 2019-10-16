namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element that returns a binding.
    /// </summary>
    public interface IBindingNode : INodeExtension
    {

        /// <summary>
        /// Gets the binding defined by the element.
        /// </summary>
        Binding Binding { get; }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> created by the binding.
        /// </summary>
        EvaluationContext Context { get; }

    }

}
