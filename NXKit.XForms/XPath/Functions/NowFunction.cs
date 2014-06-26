using System;
using System.Xml.XPath;

using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}now")]
    [XsltContextFunction("now")]
    public class NowFunction :
        XPathFunction
    {

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[] { }; }
        }

        public override int Minargs
        {
            get { return 0; }
        }

        public override int Maxargs
        {
            get { return 0; }
        }

        public override XPathResultType ReturnType
        {
            get { return XPathResultType.String; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");
        }

    }

}
