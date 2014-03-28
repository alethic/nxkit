using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NXKit.DOMEvents
{

    [Export(typeof(INodeInterfaceProvider))]
    public class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(NXNode node)
        {
            if (node is NXElement)
            {
                yield return GetOrCreate(node, () => new EventTarget((NXElement)node));
                yield return GetOrCreate(node, () => new NXEventTarget((NXElement)node));
            }

            if (node is NXDocument)
            {
                yield return GetOrCreate(node, () => new DocumentEvent((NXDocument)node));
                yield return GetOrCreate(node, () => new NXDocumentEvent((NXDocument)node));
            }
        }

    }

}
