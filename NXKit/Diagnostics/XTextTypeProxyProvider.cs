using System.Xml.Linq;

namespace NXKit.Diagnostics
{

    [TypeProxyProvider(typeof(XText))]
    public class XTextTypeProxyProvider :
        TypeProxyProviderBase<XText>
    {

        public override object Proxy(XText input) => input.Parent.Name + ">text";

    }

}
