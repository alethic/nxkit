using System;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XPath
{

    /// <summary>
    /// Encapsulates the current execution context of an XPath expression found on a <see cref="XObject"/>.
    /// </summary>
    public class XObjectXsltContext :
        NXXsltContext
    {

        readonly XObject xml;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="functionProvider"></param>
        /// <param name="xml"></param>
        public XObjectXsltContext(
            IXsltContextFunctionProvider functionProvider,
            XObject xml)
            : base(functionProvider)
        {
            if (functionProvider == null)
                throw new ArgumentNullException(nameof(functionProvider));

            this.xml = xml ?? throw new ArgumentNullException(nameof(xml));
        }

        /// <summary>
        /// Gets the attribute which establishes the XPath context.
        /// </summary>
        public XObject Xml
        {
            get { return xml; }
        }

        public override string LookupNamespace(string prefix)
        {
            // default prefix resolve to default namespace
            if (prefix == "")
                return "";

            return xml.GetNamespaceOfPrefix(prefix).NamespaceName;
        }

        public override string LookupPrefix(string uri)
        {
            // default namespace resolves to default prefix
            if (uri == "")
                return "";

            return xml.GetPrefixOfNamespace(uri);
        }

    }

}
