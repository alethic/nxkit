using System.Xml.XPath;

namespace XEngine.Forms.XForms.XPathFunctions
{

    internal class PositionFunction : XPathFunction
    {

        public override XPathResultType[] ArgTypes
        {
            get { return new XPathResultType[0]; }
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
            get { return XPathResultType.Number; }
        }

        protected override object Invoke(VisualXmlNamespaceContext context, XPathNavigator navigator, params object[] args)
        {
            return XFormsEvaluationContext.Current.Position;
        }

    }

}
