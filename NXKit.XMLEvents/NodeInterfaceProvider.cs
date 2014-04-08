using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.XmlEvents
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(XNode node)
        {
            if (node is XElement)
                if (((XElement)node).Attributes().Any(i => i.Name.Namespace == SchemaConstants.Events_1_0))
                    yield return GetOrCreate(node, () => new ElementEventListener((XElement)node));
        }

    }

}
