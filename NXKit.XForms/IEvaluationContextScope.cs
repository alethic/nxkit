namespace NXKit.XForms
{

    /// <summary>
    /// Marks a <see cref="NXNode"/> which is capable of providing an evaluation context to children.
    /// </summary>
    public interface IEvaluationContextScope
    {

        XFormsEvaluationContext Context { get; }

    }

}
