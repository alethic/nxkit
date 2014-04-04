using System.Collections.Generic;

namespace NXKit
{

    /// <summary>
    /// Overides the contents of the node.
    /// </summary>
    public interface INodeContents
    {

        IEnumerable<NXNode> GetContents();

    }

}
