using System;
using System.ComponentModel.Composition;

namespace NXKit.Scripting
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptEngineAttribute :
        ExportAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ScriptEngineAttribute()
            : base(typeof(IScriptEngine))
        {

        }

    }

}
