using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Linq;

using NXKit.XForms.Functions;
using NXKit.XPath;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a <see cref="XsltContext"/> for XForms visual operations.
    /// </summary>
    public class XFormsXsltContext :
        XsltContext
    {

        readonly NXNode node;
        readonly EvaluationContext evaluationContext;
        readonly IXsltContextFunctionProvider functionProvider;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evaluationContext"></param>
        internal XFormsXsltContext(
            NXNode node,
            EvaluationContext evaluationContext)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(evaluationContext != null);

            this.node = node;
            this.evaluationContext = evaluationContext;
            this.functionProvider = node.Document.Container.GetExportedValue<IXsltContextFunctionProvider>();
        }

        /// <summary>
        /// Gets the <see cref="Visual"/> associated with the XSLT operation.
        /// </summary>
        public NXNode Visual
        {
            get { return node; }
        }

        /// <summary>
        /// Gets the <see cref="EvaluationContext"/> associated with the XSLT operation.
        /// </summary>
        public EvaluationContext EvaluationContext
        {
            get { return evaluationContext; }
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

            var element = node.Xml as XElement;
            if (element == null)
                element = node.Parent.Xml as XElement;
            if (element == null)
                throw new NullReferenceException();

            return prefix != "" ? element.GetNamespaceOfPrefix(prefix).NamespaceName : element.GetDefaultNamespace().NamespaceName;
        }

        public override string LookupPrefix(string namespaceName)
        {
            Contract.Requires<ArgumentNullException>(namespaceName != null);

            var element = node.Xml as XElement;
            if (element == null)
                element = node.Parent.Xml as XElement;
            if (element == null)
                throw new NullReferenceException();

            return element.GetPrefixOfNamespace(namespaceName);
        }

        public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
        {
            return functionProvider.GetFunctions()
                .Where(i => XName.Get(i.Metadata.ExpandedName) == ((XNamespace)LookupNamespace(prefix) + name))
                .Select(i => i.Value)
                .FirstOrDefault();
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            throw new NotImplementedException();
        }

    }

}
