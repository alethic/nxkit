using System.IO;

namespace NXKit
{

    /// <summary>
    /// Provides an interface for the forms processor to obtain and save resources.
    /// </summary>
    public interface IResolver
    {

        Stream Get(string href, string baseUri);

        Stream Put(string href, string baseUri, Stream stream);

    }

}
