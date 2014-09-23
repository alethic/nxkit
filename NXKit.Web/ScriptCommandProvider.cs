using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.Composition;
using NXKit.Web.Commands;
using NXKit.Xml;

namespace NXKit.Web
{

    [Export(typeof(ScriptCommandProvider))]
    [Export(typeof(ICommandProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class ScriptCommandProvider :
        ICommandProvider
    {

        class DocumentScriptState
        {

            readonly List<Script> commands;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public DocumentScriptState()
            {
                commands = new List<Script>();
            }

            /// <summary>
            /// Gets the outstanding commands.
            /// </summary>
            /// <returns></returns>
            public IEnumerable<Script> GetCommands()
            {
                var all = commands.ToArray();
                commands.Clear();
                return all;
            }

            /// <summary>
            /// Adds a new command.
            /// </summary>
            /// <param name="command"></param>
            public void AddCommand(Script command)
            {
                commands.Add(command);
            }

        }

        readonly Func<Document> document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        [ImportingConstructor]
        public ScriptCommandProvider(Func<Document> document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        public IEnumerable<Command> GetCommands()
        {
            return document().Root.AnnotationOrCreate<DocumentScriptState>().GetCommands();
        }

    }

}
