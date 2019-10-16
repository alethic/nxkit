using System;

namespace NXKit.Composition
{

    /// <summary>
    /// Marks the class, method or property as exportable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExportAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        public ExportAttribute(Type type, CompositionScope scope = CompositionScope.Global) :
            this(scope)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scope"></param>
        public ExportAttribute(CompositionScope scope = CompositionScope.Global)
        {
            Scope = scope;
        }

        /// <summary>
        /// Gets the type this object is exported as.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Scope at which the object is registered.
        /// </summary>
        public CompositionScope Scope { get; }

        /// <summary>
        /// Metadata associated with the export.
        /// </summary>
        public object Key { get; set; }

    }

}
