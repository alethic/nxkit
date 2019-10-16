using System.Xml.Xsl;

using NXKit.Composition;

namespace NXKit.XPath
{

    /// <summary>
    /// Describes an exported <see cref="IXsltContextFunction"/>.
    /// </summary>
    public interface IXsltContextFunctionMetadata : IExportMetadata
    {

        /// <summary>
        /// Gets the fully qualified name of the function.
        /// </summary>
        string[] ExpandedName { get; }

    }

}
