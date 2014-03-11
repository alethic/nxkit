using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    public abstract class Module
    {

        INXDocument engine;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Module()
        {

        }

        /// <summary>
        /// Override to specify the modules this module depends on.
        /// </summary>
        public virtual Type[] DependsOn
        {
            get { return new Type[0]; }
        }

        /// <summary>
        /// Initializes the module instance against the specified <see cref="Engine"/>.
        /// </summary>
        /// <param name="engine"></param>
        public virtual void Initialize(NXDocument engine)
        {
            Contract.Requires<ArgumentNullException>(engine != null);

            this.engine = engine;
        }

        /// <summary>
        /// Gets a reference to the form processor hosting this module.
        /// </summary>
        public INXDocument Engine
        {
            get { return engine; }
        }

        /// <summary>
        /// Invoked when the engine wants to create a visual. Override this method to implement creation of <see
        /// cref="Visual"/> instances. Return <c>null</c> if the module doesn't support generation of a <see 
        /// cref="Visual"/> for the given element name.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract Visual CreateVisual(XName xname);

        /// <summary>
        /// Gives a <see cref="Module"/> a chance to attach additional information to a <see cref="Visual"/> created by
        /// other modules.
        /// </summary>
        /// <param name="visual"></param>
        public virtual void AnnotateVisual(Visual visual)
        {

        }

        /// <summary>
        /// Invokes the module.
        /// </summary>
        /// <returns></returns>
        public virtual bool Invoke()
        {
            return false;
        }

    }

}
