using System.Xml.XPath;

using NXKit.Diagnostics;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}boolean-from-string")]
    public class BooleanFromString :
        XPathFunction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="trace"></param>
        public BooleanFromString(ITraceService trace) :
            base(trace)
        {

        }

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
            get { return XPathResultType.Boolean; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            bool result;
            if (bool.TryParse(args[0].ToString(), out result))
                return result;
            else
                return false;
        }
    }

}
