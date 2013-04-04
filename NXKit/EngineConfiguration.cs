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

        List<Type> moduleTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EngineConfiguration()
        {

        }

        /// <summary>
        /// Set of modules to make available.
        /// </summary>
        public IEnumerable<Type> ModuleTypes
        {
            get { return moduleTypes; }
        }

        /// <summary>
        /// Adds the given module type to the configuration.
        /// </summary>
        public void AddModule(Type type)
        {
            if (!typeof(Module).IsAssignableFrom(type))
                throw new ArgumentException("Type must be NXKit.Module type.", "type");

            moduleTypes.Add(type);
        }

        /// <summary>
        /// Adds the given module type to the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddModule<T>()
            where T : Module, new()
        {
            moduleTypes.Add(typeof(T));
        }

    }

}
