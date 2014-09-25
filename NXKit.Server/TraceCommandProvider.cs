using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;
using NXKit.Server.Commands;

namespace NXKit.Server
{

    [Export(typeof(TraceCommandProvider))]
    [Export(typeof(ICommandProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class TraceCommandProvider :
        ICommandProvider
    {

        readonly TraceSink sink;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sink"></param>
        [ImportingConstructor]
        public TraceCommandProvider(TraceSink sink)
        {
            Contract.Requires<ArgumentNullException>(sink != null);

            this.sink = sink;
        }

        IEnumerable<Command> ICommandProvider.Commands
        {
            get { return sink.Messages.Select(i => new Trace(i)); }
        }

    }

}
