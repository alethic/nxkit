using System;
using System.Xml.XPath;

using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}days-from-date")]
    [XsltContextFunction("days-from-date")]
    public class DaysFromDateFunction :
        XPathFunction
    {

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[] { XPathResultType.String }; }
        }

        public override int Minargs
        {
            get { return 1; }
        }

        public override int Maxargs
        {
            get { return 1; }
        }

        public override XPathResultType ReturnType
        {
            get { return XPathResultType.Number; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            DateTime d;
            if (!DateTime.TryParse((string)args[0], out d))
                return double.NaN;

            return (d - new DateTime(1970, 1, 1)).TotalDays;
        }

    }

}
