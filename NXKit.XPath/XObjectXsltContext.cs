using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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
        [ImportingConstructor]
        public XObjectXsltContext(
            IXsltContextFunctionProvider functionProvider,
            XObject xml)
            : base(functionProvider)
        {
            Contract.Requires<ArgumentNullException>(functionProvider != null);
            Contract.Requires<ArgumentNullException>(xml != null);

            this.xml = xml;
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
            return xml.GetNamespaceOfPrefix(prefix).NamespaceName;
        }

        public override string LookupPrefix(string uri)
        {
            return xml.GetPrefixOfNamespace(uri);
        }

    }

}
