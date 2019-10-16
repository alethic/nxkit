using System;
using System.Collections.Generic;
using System.Xml.Xsl;

using NXKit.Composition;

namespace NXKit.XPath
{

    /// <summary>
    /// Provides XSLT functions.
    /// </summary>
    [XsltContextFunctionProvider]
    public class DefaultXsltContextFunctionProvider :
        IXsltContextFunctionProvider
    {

        readonly IEnumerable<IExport<IXsltContextFunction, IXsltContextFunctionMetadata>> functions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="functions"></param>
        public DefaultXsltContextFunctionProvider(IEnumerable<IExport<IXsltContextFunction, IXsltContextFunctionMetadata>> functions)
        {
            this.functions = functions ?? throw new ArgumentNullException(nameof(functions));
        }

        public IEnumerable<IExport<IXsltContextFunction, IXsltContextFunctionMetadata>> GetFunctions()
        {
            return functions;
        }

    }

}
