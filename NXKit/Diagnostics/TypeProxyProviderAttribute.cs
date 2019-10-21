using System;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeProxyProviderAttribute :
        ExportAttribute,
        ITypeProxyProviderMetadata
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="proxyType"></param>
        public TypeProxyProviderAttribute(Type proxyType) :
            base(typeof(ITypeProxyProvider))
        {
            ProxyType = proxyType ?? throw new ArgumentNullException(nameof(proxyType));
        }

        public Type ProxyType { get;  }

    }

}
