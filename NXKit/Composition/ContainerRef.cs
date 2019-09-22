using System;
using System.ComponentModel.Composition.Hosting;

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
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public CompositionContainer Container
        {
            get { return container; }
        }

    }

}
