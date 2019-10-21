using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Util;

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
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.Recurse(i => i.BaseType);
        }

        /// <summary>
        /// Gets all implemented types for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllTypes(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetTypes(type).Concat(type.GetInterfaces());
        }

    }

}
