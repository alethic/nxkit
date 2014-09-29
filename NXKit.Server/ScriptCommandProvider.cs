using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.Composition;
using NXKit.IO.Media;
using NXKit.Server.Commands;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.Server
{

    [Export(typeof(ScriptCommandProvider))]
    [Export(typeof(ICommandProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class ScriptCommandProvider :
        ICommandProvider
    {

        class State
        {

            internal readonly Queue<Script> scripts;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public State()
            {
                scripts = new Queue<Script>();
            }

        }

        readonly Func<Document> document;
        readonly Lazy<State> state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        [ImportingConstructor]
        public ScriptCommandProvider(Func<Document> document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
            this.state = new Lazy<State>(() => document().Xml.AnnotationOrCreate<State>());
        }

        /// <summary>
        /// Adds a script to be delivered and executed on the client.
        /// </summary>
        /// <param name="code"></param>
        public void Add(string code)
        {
            state.Value.scripts.Enqueue(new Script(code));
        }

        /// <summary>
        /// Adds a script to be delivered and executed on the client.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        public void Add(MediaRange type, string code)
        {
            state.Value.scripts.Enqueue(new Script(type, code));
        }

        IEnumerable<Command> ICommandProvider.Commands
        {
            get { return state.Value.scripts.DequeueAll(); }
        }

    }

}
