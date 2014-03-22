using System.Xml.XPath;

namespace NXKit.XForms.XPathFunctions
{

    internal class IfFunction : XPathFunction
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

        protected override object Invoke(XFormsXsltContext context, XPathNavigator navigator, params object[] args)
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
