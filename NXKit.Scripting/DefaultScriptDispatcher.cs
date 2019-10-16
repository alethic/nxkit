using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;

namespace NXKit.Scripting
{

    [Export(typeof(IScriptDispatcher), CompositionScope.Host)]
    public class DefaultScriptDispatcher :
        IScriptDispatcher
    {

        readonly IEnumerable<IScriptEngine> engines;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="engines"></param>
        public DefaultScriptDispatcher(
            IEnumerable<IScriptEngine> engines)
        {
            this.engines = engines ?? throw new ArgumentNullException(nameof(engines));
        }

        /// <summary>
        /// Gets the <see cref="IScriptEngine"/>s that support the specified language type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IEnumerable<IScriptEngine> GetEngines(string type, string code)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (code == null)
                throw new ArgumentNullException(nameof(code));

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
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (code == null)
                throw new ArgumentNullException(nameof(code));

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
