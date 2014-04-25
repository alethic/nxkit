using System.Xml.Linq;

namespace NXKit.Diagnostics
{

    [TypeProxyProvider(typeof(XDocument))]
    public class XDocumentTypeProxyProvider :
        TypeProxyProviderBase<XDocument>
    {

        public override object Proxy(XDocument input)
        {
            return "XDocument";
        }

    }

}
