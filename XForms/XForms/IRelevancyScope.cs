namespace ISIS.Forms.XForms
{

    /// <summary>
    /// Indicates a <see cref="Visual"/> whose relevancy applies to descendant controls.
    /// </summary>
    public interface IRelevancyScope
    {

        bool Relevant { get; }

    }

}
