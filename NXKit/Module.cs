using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    [ContractClass(typeof(Module_Contract))]
    public abstract class Module
    {

        INXDocument document;

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
            get { Contract.Ensures(Contract.Result<Type[]>() != null); return new Type[0]; }
        }

        /// <summary>
        /// Initializes the module instance against the specified <see cref="Document"/>.
        /// </summary>
        /// <param name="document"></param>
        public virtual void Initialize(NXDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        /// <summary>
        /// Gets a reference to the form processor hosting this module.
        /// </summary>
        public INXDocument Document
        {
            get { return document; }
        }

        /// <summary>
        /// Invoked when the engine wants to create a visual. Override this method to implement creation of <see
        /// cref="Visual"/> instances. Return <c>null</c> if the module doesn't support generation of a <see 
        /// cref="Visual"/> for the given element name.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual Visual CreateVisual(XName xname)
        {
            Contract.Requires<ArgumentNullException>(xname != null);
            return null;
        }

        /// <summary>
        /// Gives a <see cref="Module"/> a chance to attach additional information to a <see cref="Visual"/> created by
        /// other modules.
        /// </summary>
        /// <param name="visual"></param>
        public virtual void AnnotateVisual(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        /// <summary>
        /// Gets the supported interfaces for the given <see cref="Visual"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetInterfaces(Visual visual)
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            yield break;
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

    [ContractClassFor(typeof(Module))]
    abstract class Module_Contract :
        Module
    {

        public override Visual CreateVisual(XName xname)
        {
            Contract.Requires<ArgumentNullException>(xname != null);
            throw new NotImplementedException();
        }

    }

}
