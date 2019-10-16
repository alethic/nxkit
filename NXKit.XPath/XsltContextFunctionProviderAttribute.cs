using System;

using NXKit.Composition;

namespace NXKit.XPath
{

    /// <summary>
    /// Exports a function provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class XsltContextFunctionProviderAttribute :
        ExportAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XsltContextFunctionProviderAttribute()
            : base(typeof(IXsltContextFunctionProvider))
        {

        }

    }

}
