using System;

using NXKit.Composition;

namespace NXKit.Autofac
{

    class Export<TValue, TMetadata> : Export<TValue>, IExport<TValue, TMetadata>
        where TMetadata : IExportMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="metadata"></param>
        public Export(Func<TValue> value, TMetadata metadata) :
            base(value)
        {
            Metadata = metadata;
        }

        public TMetadata Metadata { get; }

    }


    class Export<TValue> : IExport<TValue>
    {

        readonly Func<TValue> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Export(Func<TValue> value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public TValue Value => value();

    }

}
