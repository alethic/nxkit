using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.View.Js
{

    /// <summary>
    /// Attachs an <see cref="IViewModule"/> instance to each element.
    /// </summary>
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Extension(ExtensionObjectType.Element | ExtensionObjectType.Text)]
    [Remote]
    public class ViewModule :
        NodeExtension
    {

        readonly IEnumerable<IViewModuleDependencyProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="providers"></param>
        [ImportingConstructor]
        public ViewModule(
            XNode node,
            [ImportMany] IEnumerable<IViewModuleDependencyProvider> providers)
            : base(node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(providers != null);

            this.providers = providers;
        }

        [Remote]
        public IEnumerable<ViewModuleDependency> Require
        {
            get { return providers.SelectMany(i => i.GetDependencies(Node)); }
        }

    }

}
