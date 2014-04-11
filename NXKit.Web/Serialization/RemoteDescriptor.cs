using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    public class RemoteDescriptor
    {

        readonly object target;
        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public RemoteDescriptor(object target, Type type)
        {
            Contract.Requires<ArgumentNullException>(target != null);
            Contract.Requires<ArgumentNullException>(type != null);

            this.target = target;
            this.type = type;
        }

        /// <summary>
        /// Gets the target of the extension.
        /// </summary>
        public object Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets the type of the extension.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

    }

}
