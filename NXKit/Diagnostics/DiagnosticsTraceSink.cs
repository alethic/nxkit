using System.ComponentModel.Composition;
using System.Diagnostics;

using NXKit.Composition;

namespace NXKit.Diagnostics
{

    [Export(typeof(ITraceSink))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Global)]
    public class DiagnosticsTraceSink :
        ITraceSink
    {

        static readonly TraceSource trace = new TraceSource("NXKit");

        public void Debug(object data)
        {
            trace.TraceEvent(TraceEventType.Verbose, 0, data.ToString());
        }

        public void Debug(string message)
        {
            trace.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        public void Debug(string format, params object[] args)
        {
            trace.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public void Information(object data)
        {
            trace.TraceEvent(TraceEventType.Information, 0, data.ToString());
        }

        public void Information(string message)
        {
            trace.TraceEvent(TraceEventType.Information, 0, message);
        }

        public void Information(string format, params object[] args)
        {
            trace.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        public void Warning(object data)
        {
            trace.TraceEvent(TraceEventType.Warning, 0, data.ToString());
        }

        public void Warning(string message)
        {
            trace.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public void Warning(string format, params object[] args)
        {
            trace.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

    }

}
