using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(functions != null);

            this.functions = functions;
        }

        public IEnumerable<Lazy<IXsltContextFunction, IXsltContextFunctionMetadata>> GetFunctions()
        {
            return functions;
        }

    }

}
