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
    /// <typeparam name="TExtension"></typeparam>
    [Export(typeof(IExtensionService<>))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ExtensionService<TExtension> :
        IExtensionService<TExtension>
        where TExtension : IExtension
    {

        readonly XObject obj;
        readonly IEnumerable<IExtensionProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="providers"></param>
        [ImportingConstructor]
        public ExtensionService(
            XObject obj,
            [ImportMany] IEnumerable<IExtensionProvider> providers)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(providers != null);

            this.obj = obj;
            this.providers = providers;
        }

        /// <summary>
        /// Exports the extension value.
        /// </summary>
        public TExtension Value
        {
            get { return GetValue(); }
        }

        TExtension GetValue()
        {
            return providers
                .SelectMany(i => i.GetExtensions<TExtension>(obj))
                .FirstOrDefault();
        }

    }
}
