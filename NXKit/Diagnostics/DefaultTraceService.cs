using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    [Export(typeof(ITraceService), CompositionScope.Host)]
    public class DefaultTraceService :
        ITraceService
    {

        readonly IEnumerable<ITraceSink> sinks;
        readonly IEnumerable<IExport<ITypeProxyProvider, ITypeProxyProviderMetadata>> proxies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sinks"></param>
        public DefaultTraceService(
            IEnumerable<ITraceSink> sinks,
            IEnumerable<IExport<ITypeProxyProvider, ITypeProxyProviderMetadata>> proxies)
        {
            this.sinks = sinks ?? throw new ArgumentNullException(nameof(sinks));
            this.proxies = proxies ?? throw new ArgumentNullException(nameof(proxies));
        }

        object GetProxy(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return proxies
                .Where(i => i.Metadata.ProxyType.IsInstanceOfType(input))
                .Select(i => i.Value)
                .Select(i => i.Proxy(input))
                .FirstOrDefault(i => i != null) ?? input;
        }

        object[] GetProxies(params object[] inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));

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
                sink.Information(GetProxy(data));
        }

        public void Information(string message)
        {
            foreach (var sink in sinks)
                sink.Information(message);
        }

        public void Information(string format, params object[] args)
        {
            foreach (var sink in sinks)
                sink.Information(format, GetProxies(args));
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

        public void Error(object data)
        {
            foreach (var sink in sinks)
                sink.Error(GetProxy(data));
        }

        public void Error(string message)
        {
            foreach (var sink in sinks)
                sink.Error(message);
        }

        public void Error(string format, params object[] args)
        {
            foreach (var sink in sinks)
                sink.Error(format, GetProxies(args));
        }

    }

}
