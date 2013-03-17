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

        protected override object Invoke(VisualXmlNamespaceContext context, XPathNavigator navigator, params object[] args)
        {
            if (args[0] as bool? == true)
                return args[1];
            else
                return args[2];
        }

    }

}
