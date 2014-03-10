using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

using NXKit.XForms.XPathFunctions;

namespace NXKit.XForms
{

    /// <summary>
    /// Proxies the namespace context of a node to other systems.
    /// </summary>
    public class XFormsXsltContext :
        XsltContext
    {

        readonly Visual visual;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        internal XFormsXsltContext(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            this.visual = visual;
        }

        public Visual Visual
        {
            get { return visual; }
        }

        public override bool Whitespace
        {
            get { return true; }
        }

        public override bool PreserveWhitespace(XPathNavigator node)
        {
            return true;
        }

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        public override string LookupNamespace(string prefix)
        {
            Contract.Requires<ArgumentNullException>(prefix != null);

            var element = visual.Node as XElement;
            if (element == null)
                element = visual.Parent.Element;

            if (element == null)
                throw new NullReferenceException();

            return prefix != "" ? element.GetNamespaceOfPrefix(prefix).NamespaceName : element.GetDefaultNamespace().NamespaceName;
        }

        public override string LookupPrefix(string namespaceName)
        {
            Contract.Requires<ArgumentNullException>(namespaceName != null);

            var element = visual.Node as XElement;
            if (element == null)
                element = visual.Parent.Element;

            if (element == null)
                throw new NullReferenceException();

            return element.GetPrefixOfNamespace(namespaceName);
        }

        public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
        {
            var ns = (XNamespace)LookupNamespace(prefix);
            if (ns == Constants.XForms_1_0)
            {
                switch (name)
                {
                    case "instance":
                        return new InstanceFunction();
                }
            }

            return null;
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            throw new NotImplementedException();
        }

    }

}
