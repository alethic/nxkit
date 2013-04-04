using System;
using System.Collections.Generic;

namespace NXKit
{

    /// <summary>
    /// Provides configuration to the NXKit engine.
    /// </summary>
    [Serializable]
    public class EngineConfiguration
    {

        /// <summary>
        /// Set of modules to make available.
        /// </summary>
        internal List<Type> ModuleTypes { get; private set; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EngineConfiguration()
        {
            ModuleTypes = new List<Type>();
        }

        /// <summary>
        /// Adds the given module type to the configuration.
        /// </summary>
        public void AddModule(Type type)
        {
            if (!typeof(Module).IsAssignableFrom(type))
                throw new ArgumentException("Type must be NXKit.Module type.", "type");

            ModuleTypes.Add(typeof(T));
        }

        /// <summary>
        /// Adds the given module type to the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddModule<T>()
            where T : Module, new()
        {
            ModuleTypes.Add(typeof(T));
        }

    }

}
