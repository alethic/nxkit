namespace NXKit.Scripting
{

    /// <summary>
    /// Provides an interface towards executing scripts within the document.
    /// </summary>
    public interface IDocumentScript : IDocumentExtension
    {

        object Execute(string type, string code);

    }

}
