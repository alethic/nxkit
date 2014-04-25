using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace NXKit.Composition
{

    /// <summary>
    /// An <see cref="ExportProvider"/> implementation that returns no exports.
    /// </summary>
    class EmptyExportProvider :
        ExportProvider
    {

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            yield break;
        }

    }

}
