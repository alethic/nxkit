using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Exports <see cref="XObject"/> extensions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Export(typeof(IInterface<>))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Interface<T> :
        IInterface<T>
        where T : class
    {

        readonly XObject obj;
        readonly IEnumerable<IInterfaceProvider> providers;
        T value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="providers"></param>
        [ImportingConstructor]
        public Interface(
            XObject obj,
            [ImportMany] IEnumerable<IInterfaceProvider> providers)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(providers != null);

            this.obj = obj;
            this.providers = providers;
        }

        /// <summary>
        /// Exports the interface value.
        /// </summary>
        public T Value
        {
            get { return value ?? (value = GetValue()); }
        }

        /// <summary>
        /// Implements the getter for Value.
        /// </summary>
        /// <returns></returns>
        [Export(typeof(Func<>))]
        public T GetValue()
        {
            return providers
                .SelectMany(i => i.GetInterfaces<T>(obj))
                .FirstOrDefault();
        }

    }
}
