using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.Serialization;
using NXKit.Xml;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Captures trace messages from the <see cref="NXDocumentHost"/> to be output to the client.
    /// </summary>
    [Export(typeof(TraceSink))]
    [Export(typeof(ITraceSink))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class TraceSink :
        ITraceSink
    {

        [SerializableAnnotation]
        [XmlRoot("trace-sink")]
        public class State :
            IXmlSerializable
        {

            internal Queue<TraceMessage> messages = new Queue<TraceMessage>();

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {

            }

            public void WriteXml(XmlWriter writer)
            {

            }

        }

        readonly Lazy<State> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public TraceSink(
            Func<NXDocumentHost> host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.state = new Lazy<State>(() => host().Xml.AnnotationOrCreate<State>());
        }

        /// <summary>
        /// Gets all the logged messages.
        /// </summary>
        public IEnumerable<TraceMessage> Messages
        {
            get { return GetMessages(); }
        }

        /// <summary>
        /// Dequeues all of the pending messages.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TraceMessage> GetMessages()
        {
            while (state.Value.messages.Count > 0)
                yield return state.Value.messages.Dequeue();
        }

        public void Debug(object data)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Verbose, data.ToString()));
        }

        public void Debug(string message)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Verbose, message));
        }

        public void Debug(string format, params object[] args)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Verbose, string.Format(format, args)));
        }

        public void Information(object data)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Information, data.ToString()));
        }

        public void Information(string message)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Information, message));
        }

        public void Information(string format, params object[] args)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Information, string.Format(format, args)));
        }

        public void Warning(object data)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Warning, data.ToString()));
        }

        public void Warning(string message)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Warning, message));
        }

        public void Warning(string format, params object[] args)
        {
            state.Value.messages.Enqueue(new TraceMessage(Severity.Warning, string.Format(format, args)));
        }

    }

}
