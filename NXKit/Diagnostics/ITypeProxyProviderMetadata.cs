using System;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    public interface ITypeProxyProviderMetadata : IExportMetadata
    {

        Type ProxyType { get; }

    }

}
