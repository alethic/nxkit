namespace NXKit.XForms
{

    /// <summary>
    /// Marks a <see cref="Visual"/> which is capable of providing an evaluation context to children.
    /// </summary>
    public interface IEvaluationContextScope
    {

        XFormsEvaluationContext Context { get; }

    }

}
