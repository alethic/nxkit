using System;

namespace NXKit.Scripting
{

    /// <summary>
    /// Describes an object available to the scripting engines under a given name.
    /// </summary>
    public class ScriptObjectDescriptor
    {

        readonly string name;
        readonly Func<object> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ScriptObjectDescriptor(string name, Func<object> value)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the exposed name of the <see cref="object"/>.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the value for the <see cref="object"/>.
        /// </summary>
        public object Value
        {
            get { return value(); }
        }

    }

}
