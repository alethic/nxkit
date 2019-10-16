using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;
using NXKit.View.Server.Commands;

namespace NXKit.View.Server
{

    [Export(typeof(TraceCommandProvider), CompositionScope.Host)]
    [Export(typeof(ICommandProvider), CompositionScope.Host)]
    public class TraceCommandProvider :
        ICommandProvider
    {

        readonly TraceSink sink;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sink"></param>
        public TraceCommandProvider(TraceSink sink)
        {
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
        }

        IEnumerable<Command> ICommandProvider.Commands
        {
            get { return sink.Messages.Select(i => new Trace(i)); }
        }

    }

}
