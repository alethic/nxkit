using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.Scripting
{
    
    [Export(typeof(INodeInterfaceProvider))]
    class NodeInterfaceProvider :
        NodeInterfaceProviderBase
    {

        public override IEnumerable<object> GetInterfaces(XNode node)
        {
            if (node is XDocument)
                yield return GetOrCreate(node, () => new DocumentScript((XDocument)node));
        }

    }

}
