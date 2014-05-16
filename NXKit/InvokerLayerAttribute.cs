using System;
using System.ComponentModel.Composition;

namespace NXKit
{

    /// <summary>
    /// Exports a <see cref="IInvokerLayer"/> implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    [MetadataAttribute]
    public class InvokerLayerAttribute :
        ExportAttribute
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InvokerLayerAttribute()
            : base(typeof(IInvokerLayer))
        {

        }

    }

}