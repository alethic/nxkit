using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes a class that provides interfaces for <see cref="XObject"/>s.
    /// </summary>
    [ContractClass(typeof(IInterfaceProvider_Contract))]
    public interface IInterfaceProvider
    {

        /// <summary>
        /// Gets available interfaces for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<object> GetInterfaces(XObject obj);

    }

    [ContractClassFor(typeof(IInterfaceProvider))]
    abstract class IInterfaceProvider_Contract :
        IInterfaceProvider
    {

        public IEnumerable<object> GetInterfaces(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            throw new NotImplementedException();
        }

    }

}
