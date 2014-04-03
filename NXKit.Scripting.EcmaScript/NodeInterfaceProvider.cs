using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NXKit.Scripting.EcmaScript
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(NXNode node)
        {
            yield break;
        }

    }

}
