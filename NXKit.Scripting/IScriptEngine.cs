namespace NXKit.Scripting
{

    /// <summary>
    /// Provides a script engine for a particular language type.
    /// </summary>
    public interface IScriptEngine
    {

        /// <summary>
        /// Returns <c>true</c> if the given script engine can execute the given script.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CanExecute(string type, string code);

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="code"></param>
        object Execute(string code);

    }

}
