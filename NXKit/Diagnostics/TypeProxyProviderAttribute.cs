using System;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeProxyProviderAttribute :
        ExportAttribute,
        ITypeProxyProviderMetadata
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public TypeProxyProviderAttribute(Type type) :
            base(typeof(ITypeProxyProvider))
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type ProxyType => type;

    }

}
