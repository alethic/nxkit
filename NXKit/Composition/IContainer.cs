using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace NXKit.Composition
{

    public interface IContainer
    {

        IEnumerable<Lazy<T, TMetadata>> GetExports<T, TMetadata>(Type contractType);

        IEnumerable<Lazy<T, IDictionary<string, object>>> GetExports<T>(Type contractType);

        IEnumerable<Lazy<T>> GetExports<T>();

        IEnumerable<T> GetExportedValues<T>(Type contractType);

        IEnumerable<T> GetExportedValues<T>();

        IEnumerable<object> GetExportedValues(Type contractType);

        IEnumerable<Export> GetExports(ImportDefinition importDefinition);

        IContainer WithExport<T>(T value) 
            where T : class;

    }

}
