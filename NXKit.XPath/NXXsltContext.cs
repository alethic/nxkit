using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace NXKit.XPath
{

    /// <summary>
    /// Encapsulates the current execution context of an XPath expression.
    /// </summary>
    public class NXXsltContext :
        System.Xml.Xsl.XsltContext
    {

        readonly IXsltContextFunctionProvider functionProvider;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="functionProvider"></param>
        [ImportingConstructor]
        public NXXsltContext(
            IXsltContextFunctionProvider functionProvider)
            : base()
        {
            Contract.Requires<ArgumentNullException>(functionProvider != null);

            this.functionProvider = functionProvider;
        }

        /// <summary>
        /// Resolves the specified function name.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="localName"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public override IXsltContextFunction ResolveFunction(string prefix, string localName, XPathResultType[] argTypes)
        {
            var name = XName.Get(localName, prefix != "" ? LookupNamespace(prefix) : XNamespace.None.NamespaceName);
            if (name == null)
                throw new XPathException("Unable to resolve function name.");

            return functionProvider.GetFunctions()
                .SelectMany(i => i.Metadata.ExpandedName
                    .Select((j, k) => new
                    {
                        Name = i.Metadata.ExpandedName[k],
                        IsPrefixRequired = i.Metadata.IsPrefixRequired[k],
                        Item = i,
                    }))
                .Where(i => ResolveFunctionPredicate(
                    XName.Get(i.Name),
                    i.IsPrefixRequired,
                    name))
                .Select(i => i.Item.Value)
                .FirstOrDefault();
        }

        /// <summary>
        /// Test whether the given candidate function data matches with the requested name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isPrefixRequired"></param>
        /// <param name="requested"></param>
        /// <returns></returns>
        bool ResolveFunctionPredicate(XName name, bool isPrefixRequired, XName requested)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(requested != null);

            if (requested.LocalName != name.LocalName)
                return false;

            if (requested.NamespaceName == "")
                if (!isPrefixRequired)
                    return true;

            if (requested.NamespaceName == name.NamespaceName)
                return true;

            return false;
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            throw new NotImplementedException();
        }

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        public override bool PreserveWhitespace(XPathNavigator node)
        {
            return true;
        }

        public override bool Whitespace
        {
            get { return true; }
        }

    }

}
