namespace NXKit.XForms
{

    /// <summary>
    /// Describes an evaluation context specified explicitly on an element.
    /// </summary>
    public interface IEvaluationContextProvider
    {

        /// <summary>
        /// Gets the context.
        /// </summary>
        EvaluationContext Context { get; }

    }

}
