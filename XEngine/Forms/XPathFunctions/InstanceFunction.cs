using System.Linq;
using System.Xml.XPath;

using XEngine.Util;

namespace XEngine.Forms.XPathFunctions
{

    internal class InstanceFunction : XPathFunction
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

        protected override object Invoke(VisualXmlNamespaceContext context, XPathNavigator navigator, params object[] args)
        {
            var id = args.Length > 0 ? ((string)args[0]).TrimToNull() : null;
            if (id == null)
                return XFormsEvaluationContext.Current.Model.Instances.First().State.InstanceElement.CreateNavigator().Select(".");
            else
            {
                var instance = XFormsEvaluationContext.Current.Model.Instances.FirstOrDefault(i => i.Id == id);
                if (instance != null)
                    return instance.State.InstanceElement.CreateNavigator().Select(".");
                else
                    return null;
            }
        }

    }

}
