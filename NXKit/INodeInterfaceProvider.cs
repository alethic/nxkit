using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Describes a class that provides interfaces for <see cref="NXNode"/>s.
    /// </summary>
    [ContractClass(typeof(INodeInterfaceProvider_Contract))]
    public interface INodeInterfaceProvider
    {

        /// <summary>
        /// Gets available interfaces for the given <see cref="NXNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<object> GetInterfaces(NXNode node);

    }

    [ContractClassFor(typeof(INodeInterfaceProvider))]
    abstract class INodeInterfaceProvider_Contract :
        INodeInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            throw new NotImplementedException();
        }

    }

}
