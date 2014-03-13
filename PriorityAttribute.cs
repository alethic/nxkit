using System;
using System.Linq;
using System.Reflection;

namespace NXKit
{

    /// <summary>
    /// Marks a class as having a certain priority.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PriorityAttribute :
        Attribute
    {

        /// <summary>
        /// Gets the priority set on the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetPriority(Type type)
        {
            return type.GetCustomAttributes<PriorityAttribute>(true).Select(i => i.Priority).FirstOrDefault();
        }

        readonly int priority;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="priority"></param>
        public PriorityAttribute(int priority)
        {
            this.priority = priority;
        }

        public int Priority
        {
            get { return priority; }
        }

    }

}
