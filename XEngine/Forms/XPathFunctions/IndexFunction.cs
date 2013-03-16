using System.Xml.XPath;

namespace XEngine.Forms.XPathFunctions
{

    internal class IndexFunction : XPathFunction
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

        protected override object Invoke(VisualXmlNamespaceContext context, XPathNavigator navigator, params object[] args)
        {
            var repeatId = (string)args[0];
            if (repeatId == null)
                return double.NaN;

            var repeatVisual = (XFormsRepeatVisual)Visual.Current.ResolveId(repeatId);
            if (repeatVisual == null)
                return double.NaN;

            return repeatVisual.Index;
        }

    }

}
