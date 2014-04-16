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
            Contract.Assert(prefix != null);
            Contract.Assert(localName != null);
            Contract.Assert(argTypes != null);

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
                    prefix,
                    localName))
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
        bool ResolveFunctionPredicate(XName name, bool isPrefixRequired, string prefix, string localName)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(prefix != null);
            Contract.Requires<ArgumentNullException>(localName != null);

            // local name must match
            if (localName != name.LocalName)
                return false;

            // prefix not required
            if (prefix == "")
                if (!isPrefixRequired)
                    return true;

            // test matching namespace
            var ns = LookupNamespace(prefix);
            if (ns == name.NamespaceName)
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
