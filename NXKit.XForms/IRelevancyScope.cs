namespace NXKit.XForms
{

    /// <summary>
    /// Marks a <see cref="Visual"/> as establishing a scope of relevancy. The relevancy of the marked control, if false,
    /// affects the relevancy of children controls.
    /// </summary>
    public interface IRelevancyScope
    {

        bool Relevant { get; }

    }

}
