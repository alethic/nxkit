using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Xsl;

namespace NXKit.XPath
{

    /// <summary>
    /// Provides XSLT functions.
    /// </summary>
    [XsltContextFunctionProvider]
    public class DefaultXsltContextFunctionProvider :
        IXsltContextFunctionProvider
    {

        readonly IEnumerable<Lazy<IXsltContextFunction, IXsltContextFunctionMetadata>> functions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="functions"></param>
        [ImportingConstructor]
        public DefaultXsltContextFunctionProvider(
            [ImportMany] IEnumerable<Lazy<IXsltContextFunction, IXsltContextFunctionMetadata>> functions)
        {
            this.functions = functions ?? throw new ArgumentNullException(nameof(functions));
        }

        public IEnumerable<Lazy<IXsltContextFunction, IXsltContextFunctionMetadata>> GetFunctions()
        {
            return functions;
        }

    }

}
