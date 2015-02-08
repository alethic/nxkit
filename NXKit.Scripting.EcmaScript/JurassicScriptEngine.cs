using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.Composition;
using NXKit.IO.Media;

namespace NXKit.Scripting.EcmaScript
{

    /// <summary>
    /// Provides a ECMAScript implementation using the Jurassic JavaScript engine.
    /// </summary>
    [Export(typeof(IScriptEngine))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class JurassicScriptEngine :
        IScriptEngine,
        IDisposable
    {

        static readonly MediaRangeList ACCEPT = new MediaRange[]
        {
            "application/ecmascript",
            "application/javascript",
            "text/javascript",
        };


        readonly Func<Document> host;
        readonly Lazy<Jurassic.ScriptEngine> engine;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        [ImportingConstructor]
        public JurassicScriptEngine(Func<Document> host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
            this.engine = new Lazy<Jurassic.ScriptEngine>(() => new Jurassic.ScriptEngine());
        }

        public bool CanExecute(string type, string code)
        {
            return ACCEPT.Matches(type);
        }

        public void Execute(string type, string code)
        {
            if (!CanExecute(type, code))
                throw new InvalidOperationException();

            engine.Value.Execute(code);
        }

        public object Evaluate(string type, string code)
        {
            if (!CanExecute(type, code))
                throw new InvalidOperationException();

            return engine.Value.Evaluate(code);
        }

        public void Load()
        {

        }

        public void Save()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~JurassicScriptEngine()
        {
            Dispose();
        }

    }

}
