namespace NXKit.XForms
{

    /// <summary>
    /// Marks an extension which is capable of providing an evaluation context to children.
    /// </summary>
    public interface IEvaluationContextScope : IExtension
    {

        EvaluationContext Context { get; }

    }

}
