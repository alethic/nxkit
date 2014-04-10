namespace NXKit.Web
{

    /// <summary>
    /// Provides information for the web template.
    /// </summary>
    [Remote]
    public interface ITemplate
    {

        [Remote]
        string Name { get; }

    }

}
