using System.Xml.Linq;

namespace NXKit.Diagnostics
{

    [TypeProxyProvider(typeof(XElement))]
    public class XElementTypeProxyProvider :
        TypeProxyProviderBase<XElement>
    {

        public override object Proxy(XElement input) => input.Name;

    }

}
