using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NXKit.Web
{

    public class TraceMessage
    {

        DateTime timestamp;
        Severity severity;
        string text;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TraceMessage()
        {
            this.timestamp = DateTime.UtcNow;
            this.severity = Severity.Information;
            this.text = null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="text"></param>
        public TraceMessage(Severity severity, string text)
            : this(DateTime.UtcNow, severity, text)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="severity"></param>
        /// <param name="text"></param>
        public TraceMessage(DateTime timestamp, Severity severity, string text)
        {
            this.timestamp = timestamp;
            this.severity = severity;
            this.text = text;
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Severity Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }

}
