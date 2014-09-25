using System;
using System.Diagnostics.Contracts;

namespace NXKit.Server.Commands
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
            Contract.Requires<ArgumentNullException>(message != null);

            Message = message;
        }

        public TraceMessage Message { get; set; }

    }

}
