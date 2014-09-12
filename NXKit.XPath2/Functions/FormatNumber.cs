using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using NXKit.XPath;

namespace NXKit.XPath2.Functions
{

    [XsltContextFunction("{http://www.w3.org/2005/xpath-functions}format-number")]
    [XsltContextFunction("format-number")]
    public class FormatNumber :
        IXsltContextFunction
    {

        public XPathResultType[] ArgTypes
        {
            get
            {
                return new XPathResultType[] 
                {
                    XPathResultType.Number,
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
            get { return 2; }
        }

        public XPathResultType ReturnType
        {
            get { return XPathResultType.String; }
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
                return GetXPathValue(((XPathNodeIterator)item).OfType<XPathItem>().FirstOrDefault());

            throw new XPathException("Could not extract string value from node.");
        }

        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            Contract.Requires(args.Length >= 2);

            var input = GetXPathValue(args[0]);
            var pattern = GetXPathValue(args[1]);

            decimal number;
            if (decimal.TryParse(input, out number))
                return number.ToString(pattern);

            return null;
        }

    }

}
