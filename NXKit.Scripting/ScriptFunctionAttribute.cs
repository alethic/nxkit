using System;

namespace NXKit.Scripting
{

    /// <summary>
    /// Marks a function available to scripting languages.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptFunctionAttribute :
        Attribute
    {

        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ScriptFunctionAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ScriptFunctionAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the name of the function as known to scripting languages.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

    }

}
