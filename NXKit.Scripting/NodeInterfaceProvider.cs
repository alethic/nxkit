using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NXKit.Scripting
{
    
    [Export(typeof(INodeInterfaceProvider))]
    class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(NXNode node)
        {
            if (node is NXDocument)
                yield return GetOrCreate(node, () => new DocumentScript((NXDocument)node));
        }

    }

}
