using System;
using System.Collections.Generic;
using System.Xml.Xsl;

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
        IEnumerable<Lazy<IXsltContextFunction,IXsltContextFunctionMetadata>> GetFunctions();

    }

}
