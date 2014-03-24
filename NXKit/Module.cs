using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    public abstract class Module
    {

        NXDocument document;

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
        public NXDocument Document
        {
            get { return document; }
        }

        /// <summary>
        /// Invoked when the engine wants to create a visual. Override this method to implement creation of <see
        /// cref="NXNode"/> instances. Return <c>null</c> if the module doesn't support generation of a <see 
        /// cref="NXNode"/> for the given element name.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual NXNode CreateNode(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return null;
        }

        /// <summary>
        /// Gets the supported interfaces for the given <see cref="NXObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetInterfaces(NXObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            yield return obj;
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
