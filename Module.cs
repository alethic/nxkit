using System;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    public abstract class Module
    {

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
        public virtual void Initialize(Engine engine)
        {
            Engine = engine;
        }

        /// <summary>
        /// Gets a reference to the form processor hosting this module.
        /// </summary>
        public IEngine Engine { get; private set; }

        /// <summary>
        /// Invoked when the engine wants to create a visual. Override this method to implement creation of <see
        /// cref="Visual"/> instances.
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
        /// Runs the module.
        /// </summary>
        /// <returns></returns>
        public virtual bool Run()
        {
            return false;
        }

    }

}
