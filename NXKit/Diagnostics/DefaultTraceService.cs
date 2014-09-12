using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    [Export(typeof(ITraceService))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DefaultTraceService :
        ITraceService
    {

        readonly IEnumerable<ITraceSink> sinks;
        readonly IEnumerable<Lazy<ITypeProxyProvider, ITypeProxyProviderMetadata>> proxies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sinks"></param>
        [ImportingConstructor]
        public DefaultTraceService(
            [ImportMany] IEnumerable<ITraceSink> sinks,
            [ImportMany] IEnumerable<Lazy<ITypeProxyProvider, ITypeProxyProviderMetadata>> proxies)
        {
            Contract.Requires<ArgumentNullException>(sinks != null);
            Contract.Requires<ArgumentNullException>(proxies != null);

            this.sinks = sinks;
            this.proxies = proxies;
        }

        object GetProxy(object input)
        {
            return proxies
                .Where(i => i.Metadata.Type.IsInstanceOfType(input))
                .Select(i => i.Value)
                .Select(i => i.Proxy(input))
                .FirstOrDefault(i => i != null) ?? input;
        }

        object[] GetProxies(params object[] inputs)
        {
            return inputs
                .Select(i => GetProxy(i))
                .ToArray();
        }

        public void Debug(object data)
        {
            foreach (var sink in sinks)
                sink.Debug(GetProxy(data));
        }

        public void Debug(string message)
        {
            foreach (var sink in sinks)
                sink.Debug(message);
        }

        public void Debug(string format, params object[] args)
        {
            foreach (var sink in sinks)
                sink.Debug(format, GetProxies(args));
        }

        public void Information(object data)
        {
            foreach (var sink in sinks)
                sink.Debug(GetProxy(data));
        }

        public void Information(string message)
        {
            foreach (var sink in sinks)
                sink.Debug(message);
        }

        public void Information(string format, params object[] args)
        {
            foreach (var sink in sinks)
                sink.Debug(format, GetProxies(args));
        }

        public void Warning(object data)
        {
            foreach (var sink in sinks)
                sink.Warning(GetProxy(data));
        }

        public void Warning(string message)
        {
            foreach (var sink in sinks)
                sink.Warning(message);
        }

        public void Warning(string format, params object[] args)
        {
            foreach (var sink in sinks)
                sink.Warning(format, GetProxies(args));
        }

    }

}
