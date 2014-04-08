using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(XNode node)
        {
            yield break;
        }

    }

}
