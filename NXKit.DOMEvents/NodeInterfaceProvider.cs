using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(XNode node)
        {
            if (node is XElement)
            {
                yield return GetOrCreate(node, () => new EventTarget((XElement)node));
                yield return GetOrCreate(node, () => new NXEventTarget((XElement)node));
            }

            if (node is XDocument)
            {
                yield return GetOrCreate(node, () => new DocumentEvent((XDocument)node));
                yield return GetOrCreate(node, () => new NXDocumentEvent((XDocument)node));
            }
        }

    }

}
