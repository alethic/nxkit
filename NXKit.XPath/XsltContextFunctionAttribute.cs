using System;
using System.Xml.Linq;
using System.Xml.Xsl;

using NXKit.Composition;

namespace NXKit.XPath
{

    /// <summary>
    /// Marks a function as a function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XsltContextFunctionAttribute :
        ExportAttribute,
        IXsltContextFunctionMetadata
    {

        readonly string expandedName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public XsltContextFunctionAttribute(XName name) :
            base(typeof(IXsltContextFunction))
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            expandedName = name.ToString();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expandedName"></param>
        public XsltContextFunctionAttribute(string expandedName) :
            this(XName.Get(expandedName))
        {
            if (expandedName == null)
                throw new ArgumentNullException(nameof(expandedName));
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string[] ExpandedName
        {
            get { return new[] { expandedName }; }
        }

    }

}
