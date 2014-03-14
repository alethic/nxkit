using System;

namespace NXKit
{

    /// <summary>
    /// Marks a property as interactive.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InteractiveAttribute :
        Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InteractiveAttribute()
        {

        }

    }

}
