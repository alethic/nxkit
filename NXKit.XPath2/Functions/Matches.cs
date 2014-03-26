using System;
using System.Xml.XPath;
using System.Xml.Xsl;

using NXKit.XPath;

namespace NXKit.XPath2.Functions
{

    [XsltContextFunction("{http://www.w3.org/2005/xpath-functions}matches")]
    public class Matches :
        IXsltContextFunction
    {

        public XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[] { XPathResultType.String }; }
        }

        public int Minargs
        {
            get { return 1; }
        }

        public int Maxargs
        {
            get { return 1; }
        }

        public XPathResultType ReturnType
        {
            get { return XPathResultType.Boolean; }
        }

        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            throw new NotImplementedException();
        }

    }

}
