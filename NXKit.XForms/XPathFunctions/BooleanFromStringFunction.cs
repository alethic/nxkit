using System.Xml.XPath;

namespace NXKit.XForms.XPathFunctions
{

    internal class BooleanFromStringFunction : XPathFunction
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
            get { return XPathResultType.Boolean; }
        }

        protected override object Invoke(XFormsXsltContext context, XPathNavigator navigator, params object[] args)
        {
            bool result;
            if (bool.TryParse(args[0].ToString(), out result))
                return result;
            else
                return false;
        }
    }

}
