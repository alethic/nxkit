using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Returns the node type itself.
    /// </summary>
    [Export(typeof(INodeInterfaceProvider))]
    public class TypeNodeInterfaceProvider :
        INodeInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(XNode node)
        {
            yield return node;
        }

    }

}
