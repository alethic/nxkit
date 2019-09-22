using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace NXKit.Composition
{

    public static class ExportProviderExtensions
    {

        public static object GetExportedValue(this ExportProvider container, Type type)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return container.GetExports(type, null, null).Select(i => i.Value).FirstOrDefault();
        }

    }

}
