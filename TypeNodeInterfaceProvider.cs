using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Returns the node type itself.
    /// </summary>
    [Export(typeof(IInterfaceProvider))]
    public class TypeNodeInterfaceProvider :
        IInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(XObject obj)
        {
            yield return obj;
        }

    }

}
