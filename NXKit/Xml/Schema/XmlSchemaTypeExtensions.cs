using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

using NXKit.Util;

namespace NXKit.Xml.Schema
{

    public static class XmlSchemaTypeExtensions
    {

        /// <summary>
        /// Gets the base types for the given schema type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<XmlSchemaType> GetBaseTypes(this XmlSchemaType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.Recurse(i => i.BaseXmlSchemaType);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="source"/> type is extended from the specified type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsExtendedFrom(this XmlSchemaType source, XmlSchemaType type)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return source.GetBaseTypes().Contains(type);
        }

    }

}
