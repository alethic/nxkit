using System;

namespace NXKit.Composition
{

    public class CompositionContextRef
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        public CompositionContextRef(ICompositionContext container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public ICompositionContext Container { get; }

    }

}
