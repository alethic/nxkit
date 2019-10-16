using System.Collections.Generic;
using System.Xml.Xsl;

using NXKit.Composition;

namespace NXKit.XPath
{

    /// <summary>
    /// Provides resolution of XPath function.
    /// </summary>
    public interface IXsltContextFunctionProvider
    {

        /// <summary>
        /// Gets all of the available functions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IExport<IXsltContextFunction, IXsltContextFunctionMetadata>> GetFunctions();

    }

}
