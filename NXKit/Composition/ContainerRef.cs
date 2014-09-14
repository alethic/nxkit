using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    public class ContainerRef
    {

        readonly CompositionContainer container;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        public ContainerRef(CompositionContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            this.container = container;
        }

        public CompositionContainer Container
        {
            get { return container; }
        }

    }

}
