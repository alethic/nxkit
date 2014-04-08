using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes a class that provides interfaces for <see cref="XNode"/>s.
    /// </summary>
    [ContractClass(typeof(INodeInterfaceProvider_Contract))]
    public interface INodeInterfaceProvider
    {

        /// <summary>
        /// Gets available interfaces for the given <see cref="XNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<object> GetInterfaces(XNode node);

    }

    [ContractClassFor(typeof(INodeInterfaceProvider))]
    abstract class INodeInterfaceProvider_Contract :
        INodeInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<InvalidOperationException>(node.Host() != null);
            throw new NotImplementedException();
        }

    }

}
