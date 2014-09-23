using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Composition
{

    public static class ExportProviderExtensions
    {

        public static object GetExportedValue(this ExportProvider container, Type type)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(type != null);

            return container.GetExports(type, null, null).Select(i => i.Value).FirstOrDefault();
        }

    }

}
