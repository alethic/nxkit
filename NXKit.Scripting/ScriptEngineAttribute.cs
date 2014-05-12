using System;

using NXKit.Composition;

namespace NXKit.Scripting
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptEngineAttribute :
        ScopeExportAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ScriptEngineAttribute()
            : base(typeof(IScriptEngine), Scope.Host)
        {

        }

    }

}
