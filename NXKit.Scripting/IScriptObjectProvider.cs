using System.Collections.Generic;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides objects that are available to the scripting engines.
    /// </summary>
    public interface IScriptObjectProvider
    {

        /// <summary>
        /// Returns the available scripting objects.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ScriptObjectDescriptor> GetObjects();

    }

}
