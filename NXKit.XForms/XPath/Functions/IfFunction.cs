using System.Xml.XPath;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}if")]
    public class IfFunction :
        XPathFunction
    {

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Any, XPathResultType.Any }; }
        }

        public override int Minargs
        {
            get { return 3; }
        }

        public override int Maxargs
        {
            get { return 3; }
        }

        public override XPathResultType ReturnType
        {
            get { return XPathResultType.Number; }
        }

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            if (args.Length >= 2 &&
                args[0] as bool? == true)
                return args[1];
            else if (args.Length >= 3)
                return args[2];
            return null;
        }

    }

}
