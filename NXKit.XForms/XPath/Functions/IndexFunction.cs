using System;
using System.Xml.XPath;

using NXKit.Xml;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}index")]
    [XsltContextFunction("index")]
    public class IndexFunction : 
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
            var idRef = (IdRef)(string)args[0];
            if (idRef == null)
                return double.NaN;

            var element = context.Xml.ResolveId(idRef);
            if (element == null)
                return double.NaN;

            var repeat = element.InterfaceOrDefault<Repeat>();
            if (repeat == null)
                return double.NaN;

            return (double)repeat.Index;
        }

    }

}
