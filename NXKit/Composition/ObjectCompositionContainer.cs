using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

namespace NXKit.Composition
{

    /// <summary>
    /// Internal <see cref="CompositionContainer"/> implementation for <see cref="XObject"/>s.
    /// </summary>
    class ObjectCompositionContainer :
        CompositionContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="providers"></param>
        public ObjectCompositionContainer(params ExportProvider[] providers)
            : base(providers)
        {
            Contract.Requires<ArgumentNullException>(providers != null);
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            return base.GetExportsCore(definition, atomicComposition);
        }

    }

}
