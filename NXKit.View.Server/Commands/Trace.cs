using System;

namespace NXKit.View.Server.Commands
{

    public class Trace :
        Command
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public Trace(TraceMessage message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public TraceMessage Message { get; set; }

    }

}
