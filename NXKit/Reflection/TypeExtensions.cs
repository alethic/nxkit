using System;
using System.Collections.Generic;

namespace NXKit.Reflection
{

    public static class TypeExtensions
    {

        /// <summary>
        /// Gets the type to base type hierarchy.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

    }

}
