using System.Linq;
using System.Xml.XPath;

using NXKit.Util;

namespace NXKit.XForms.XPathFunctions
{

    public class InstanceFunction :
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
            get { return XPathResultType.NodeSet; }
        }

        protected override object Invoke(XFormsXsltContext context, XPathNavigator navigator, params object[] args)
        {
            var id = args.Length > 0 ? ((string)args[0]).TrimToNull() : null;
            if (id == null)
                return GetModel(context, navigator).DefaultEvaluationContext.Instance.Element.CreateNavigator().Select(".");
            else
                return GetModel(context, navigator).Instances
                    .Where(i => i.Id == id)
                    .Select(i => i.State.InstanceElement.CreateNavigator().Select("."))
                    .FirstOrDefault();
        }
    }

}
