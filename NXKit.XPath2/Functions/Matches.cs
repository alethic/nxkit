using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml.Xsl;

using NXKit.XPath;

namespace NXKit.XPath2.Functions
{

    [XsltContextFunction("{http://www.w3.org/2005/xpath-functions}matches")]
    [XsltContextFunction("matches")]
    public class Matches :
        IXsltContextFunction
    {

        public XPathResultType[] ArgTypes
        {
            get
            {
                return new XPathResultType[] 
                {
                    XPathResultType.String,
                    XPathResultType.String,
                    XPathResultType.String,
                };
            }
        }


        public int Minargs
        {
            get { return 2; }
        }

        public int Maxargs
        {
            get { return 3; }
        }

        public XPathResultType ReturnType
        {
            get { return XPathResultType.Boolean; }
        }

        Flags GetFlags(string flags)
        {
            Contract.Requires<ArgumentNullException>(flags != null);

            var f = Flags.None;
            foreach (var c in flags)
                if (c == 's')
                    f |= Flags.DotAll;
                else if (c == 'm')
                    f |= Flags.MultiLine;
                else if (c == 'i')
                    f |= Flags.CaseInsensitive;

            return f;
        }

        string GetXPathValue(object item)
        {
            if (item == null)
                return "";
            if (item is string)
                return (string)item;
            if (item is XPathItem)
                return ((XPathItem)item).Value;
            if (item is XPathNodeIterator)
                return ((XPathNodeIterator)item).Current != null ? ((XPathNodeIterator)item).Current.Value : "";

            throw new XPathException("Could not extract string value from node.");
        }

        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            var input = GetXPathValue(args[0]);
            var pattern = GetXPathValue(args[1]);
            var flags = args.Length >= 3 ? GetFlags((string)args[2]) : Flags.None;

            var options = RegexOptions.None;
            if (flags.HasFlag(Flags.DotAll))
                options |= RegexOptions.Singleline;
            if (flags.HasFlag(Flags.MultiLine))
                options |= RegexOptions.Multiline;
            if (flags.HasFlag(Flags.CaseInsensitive))
                options |= RegexOptions.IgnoreCase;

            return Regex.IsMatch(input, pattern, options);
        }

    }

}
