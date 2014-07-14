using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace NXKit.Composition
{

    public interface IExportProvider
    {

        IEnumerable<Export> GetExports(ExportProvider source, ImportDefinition definition, AtomicComposition atomicComposition);

    }

}
