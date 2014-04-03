namespace NXKit.Scripting
{

    /// <summary>
    /// Dispatches script execution to the appropriate engine.
    /// </summary>
    public interface IScriptDispatcher
    {

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        object Execute(string type, string code);

    }

}
