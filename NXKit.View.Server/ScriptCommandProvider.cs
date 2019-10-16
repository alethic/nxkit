using System;
using System.Collections.Generic;

using NXKit.Composition;
using NXKit.IO.Media;
using NXKit.Util;
using NXKit.View.Server.Commands;
using NXKit.Xml;

namespace NXKit.View.Server
{

    [Export(typeof(ScriptCommandProvider), CompositionScope.Host)]
    [Export(typeof(ICommandProvider), CompositionScope.Host)]
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
        public ScriptCommandProvider(Func<Document> document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
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
