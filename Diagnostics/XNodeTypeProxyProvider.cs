using System.Xml.Linq;

namespace NXKit.Diagnostics
{

    [TypeProxyProvider(typeof(XNode))]
    public class XNodeTypeProxyProvider :
        TypeProxyProviderBase<XNode>
    {

        public override object Proxy(XNode input)
        {
            var document = input as XDocument;
            if (document != null)
                return "XDocument";

            var element = input as XElement;
            if (element != null)
                return "XElement: " + element.Name;

            return null;
        }

    }

}
