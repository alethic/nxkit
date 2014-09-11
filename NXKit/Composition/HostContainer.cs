using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    class HostContainer :
        Container,
        IHostContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="catalog"></param>
        public HostContainer(CompositionContainer container, ComposablePartCatalog catalog)
            : base(container, catalog)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(catalog != null);

            container.WithExport<IHostContainer>(this);
        }

    }

}
