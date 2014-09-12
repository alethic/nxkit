using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace NXKit.Composition
{

    class ObjectCatalogExportProvider :
        CatalogExportProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="catalog"></param>
        public ObjectCatalogExportProvider(ComposablePartCatalog catalog)
            : base(catalog)
        {

        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            return base.GetExportsCore(definition, atomicComposition);
        }

    }

}
