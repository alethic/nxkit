namespace XEngine.Forms
{

    /// <summary>
    /// Describes a <see cref="Visual"/> which is capable of providing an evaluation context.
    /// </summary>
    public interface IEvaluationContextScope
    {

        XFormsEvaluationContext Context { get; }

    }

}
