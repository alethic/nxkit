using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes a class that provides extensions for <see cref="XObject"/>s.
    /// </summary>
    [ContractClass(typeof(IExtensionProvider_Contract))]
    public interface IExtensionProvider
    {

        /// <summary>
        /// Gets available extensions for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IEnumerable<T> GetExtensions<T>(XObject obj);

        /// <summary>
        /// Gets available extensions for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetExtensions(XObject obj, Type type);

    }

    [ContractClassFor(typeof(IExtensionProvider))]
    abstract class IExtensionProvider_Contract :
        IExtensionProvider
    {

        public IEnumerable<T> GetExtensions<T>(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetExtensions(XObject obj, Type type)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            throw new NotImplementedException();
        }

    }

}
