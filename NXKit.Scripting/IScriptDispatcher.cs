using System;
using System.Diagnostics.Contracts;

namespace NXKit.Scripting
{

    /// <summary>
    /// Dispatches script execution to the appropriate engine depending on the language.
    /// </summary>
    [ContractClass(typeof(IScriptDispatcher_Contract))]
    public interface IScriptDispatcher
    {

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        object Evaluate(string type, string code);

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        void Execute(string type, string code);

        /// <summary>
        /// Signals any engines to load their state.
        /// </summary>
        void Load();

        /// <summary>
        /// Signals any engines to save their state.
        /// </summary>
        void Save();

    }

    [ContractClassFor(typeof(IScriptDispatcher))]
    abstract class IScriptDispatcher_Contract :
        IScriptDispatcher
    {

        public void Execute(string type, string code)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(type));
            Contract.Requires<ArgumentNullException>(code != null);
        }

        public object Evaluate(string type, string code)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(type));
            Contract.Requires<ArgumentNullException>(code != null);
            throw new NotImplementedException();
        }

        public void Load()
        {

        }

        public void Save()
        {

        }

    }

}
