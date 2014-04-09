using System;

namespace NXKit
{

    /// <summary>
    /// Marks an object or property as being available to remote clients.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public class RemoteAttribute :
        Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RemoteAttribute()
        {

        }

    }

}
