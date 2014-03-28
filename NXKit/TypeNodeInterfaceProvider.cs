using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NXKit
{

    /// <summary>
    /// Returns the node type itself.
    /// </summary>
    [Export(typeof(INodeInterfaceProvider))]
    public class TypeNodeInterfaceProvider :
        INodeInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(NXNode node)
        {
            yield return node;
        }

    }

}
