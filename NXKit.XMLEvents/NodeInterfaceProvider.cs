using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

namespace NXKit.XmlEvents
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(NXNode node)
        {
            if (node is NXElement)
                if (((NXElement)node).Attributes().Any(i => i.Name.Namespace == SchemaConstants.Events_1_0))
                    yield return new ElementEventListener((NXElement)node);
        }

    }

}
