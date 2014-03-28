using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Modules provide the implementation of a specification.
    /// </summary>
    [InheritedExport(typeof(Module))]
    public abstract class Module
    {

        readonly NXDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public Module(
            NXDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public virtual void Initialize()
        {

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
        /// Invokes the module.
        /// </summary>
        /// <returns></returns>
        public virtual bool Invoke()
        {
            return false;
        }

    }

}
