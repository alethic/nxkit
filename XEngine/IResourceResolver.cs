using System.IO;

namespace XEngine.Forms
{

    /// <summary>
    /// Provides an interface for the forms processor to obtain and save resources.
    /// </summary>
    public interface IResourceResolver
    {

        Stream Get(string href, string baseUri);

        Stream Put(string href, string baseUri, Stream stream);

    }

}
