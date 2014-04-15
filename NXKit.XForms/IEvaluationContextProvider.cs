namespace NXKit.XForms
{

    /// <summary>
    /// Describes an interface on an element that directly returns an evaluation context.
    /// </summary>
    public interface IEvaluationContextProvider
    {

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/>.
        /// </summary>
        EvaluationContext Context { get; }

    }

}
