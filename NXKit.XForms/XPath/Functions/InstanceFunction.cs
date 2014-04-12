using System.Linq;
using System.Xml.XPath;

using NXKit.DOMEvents;
using NXKit.Util;
using NXKit.Xml;
using NXKit.XPath;

namespace NXKit.XForms.XPath.Functions
{

    [XsltContextFunction("{http://www.w3.org/2002/xforms}instance")]
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

        protected override object Invoke(EvaluationXsltContext context, XPathNavigator navigator, params object[] args)
        {
            var id = args.Length > 0 ? ((string)args[0]).TrimToNull() : null;
            if (id == null)
                return GetModel(context, navigator).Instances
                    .Select(i => i.State.Document.Root.CreateNavigator().Select("."))
                    .FirstOrDefault();
            else
            {
                // resolve instance based on id
                var instance = context.Xml.ResolveId(id);
                if (instance == null)
                    throw new DOMTargetEventException(context.Xml.Parent, Events.BindingException);

                return instance
                    .Interfaces<Instance>()
                    .Select(i => i.State.Document.Root.CreateNavigator().Select("."))
                    .FirstOrDefault();
            }
        }

    }

}
