using System;

namespace NXKit
{

    /// <summary>
    /// Marks an object as being exported and available for access remotely.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
    public class PublicAttribute :
        Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public PublicAttribute()
        {

        }

    }

}
