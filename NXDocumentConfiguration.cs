using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Provides configuration to the NXKit engine.
    /// </summary>
    [Serializable]
    public class NXDocumentConfiguration
    {

        readonly List<Type> moduleTypes;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        void ObjectInvariant()
        {
            Contract.Invariant(moduleTypes != null);
        }


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXDocumentConfiguration()
        {
            this.moduleTypes = new List<Type>();
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
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentException>(typeof(Module).IsAssignableFrom(type), "Type must be NXKit.Modue type.");

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
