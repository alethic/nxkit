using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;

namespace NXKit.Scripting
{

    [Export(typeof(IScriptDispatcher))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DefaultScriptDispatcher :
        IScriptDispatcher
    {

        readonly IEnumerable<IScriptEngine> engines;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="engines"></param>
        [ImportingConstructor]
        public DefaultScriptDispatcher(
            [ImportMany] IEnumerable<IScriptEngine> engines)
        {
            Contract.Requires<ArgumentNullException>(engines != null);

            this.engines = engines;
        }

        /// <summary>
        /// Gets the <see cref="IScriptEngine"/>s that support the specified language type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IEnumerable<IScriptEngine> GetEngines(string type, string code)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(code != null);

            return engines.Where(i => i.CanExecute(type, code));
        }

        /// <summary>
        /// Gets the <see cref="IScriptEngine"/> that supports the specified language type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IScriptEngine GetEngine(string type, string code)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(code != null);

            return GetEngines(type, code).FirstOrDefault();
        }

        /// <summary>
        /// Evaluates the given code.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public object Evaluate(string type, string code)
        {
            var engine = GetEngines(type, code).FirstOrDefault();
            if (engine == null)
                throw new ScriptException(string.Format("No ScriptEngine for {0}", type));

            return engine.Evaluate(type, code);
        }

        /// <summary>
        /// Executes the given code.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        public void Execute(string type, string code)
        {
            var engine = GetEngines(type, code).FirstOrDefault();
            if (engine == null)
                throw new ScriptException(string.Format("No ScriptEngine for {0}", type));

            engine.Execute(type, code);
        }

        /// <summary>
        /// Initiates a load of any engines.
        /// </summary>
        public void Load()
        {
            foreach (var engine in engines)
                engine.Load();
        }

        /// <summary>
        /// Initiates a save of any engines.
        /// </summary>
        public void Save()
        {
            foreach (var engine in engines)
                engine.Save();
        }

    }

}
