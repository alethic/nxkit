using System;

using NXKit.Composition;

namespace NXKit.Scripting
{

    /// <summary>
    /// Exports the given object as a script object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptObjectAttribute :
        ExportAttribute,
        IScriptObjectMetadata
    {

        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ScriptObjectAttribute(string name, CompositionScope scope = CompositionScope.Global)
            : base(typeof(IScriptObject), scope)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

    }

}
