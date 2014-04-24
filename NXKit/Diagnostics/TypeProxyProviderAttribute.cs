using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

namespace NXKit.Diagnostics
{

    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public class TypeProxyProviderAttribute :
        ExportAttribute,
        ITypeProxyProviderMetadata
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public TypeProxyProviderAttribute(Type type)
            : base(typeof(ITypeProxyProvider))
        {
            Contract.Requires<ArgumentNullException>(type != null);

            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }

    }

}
