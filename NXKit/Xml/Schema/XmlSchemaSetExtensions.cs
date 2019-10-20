using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using NXKit.Util;

namespace NXKit.Xml.Schema
{

    /// <summary>
    /// Extensions methods for working with <see cref="XmlSchemaSet"/> types.
    /// </summary>
    public static class XmlSchemaSetExtensions
    {

        /// <summary>
        /// Returns the element with the specified name.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlSchemaElement GetElement(this XmlSchemaSet schema, XmlQualifiedName name)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return schema.GlobalElements.Values
                 .OfType<XmlSchemaElement>()
                 .FirstOrDefault(i => i.QualifiedName == name);
        }

        /// <summary>
        /// Returns the element with the specified name.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlSchemaElement GetElement(this XmlSchemaSet schema, XName name)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return GetElement(schema, name.ToXmlQualifiedName());
        }

        /// <summary>
        /// Returns the known elements which are members of the given element's substitution group.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<XmlSchemaElement> GetSubstitutionGroupMembers(this XmlSchemaSet schema, XmlSchemaElement element)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return schema.GlobalElements.Values
                .OfType<XmlSchemaElement>()
                .Where(i => i.SubstitutionGroup == element.QualifiedName);
        }

        /// <summary>
        /// Returns all known elements which are possible substitutions for the given element.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<XmlSchemaElement> GetSubstitutionElements(this XmlSchemaSet schema, XmlSchemaElement element)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return schema.GetSubstitutionGroupMembers(element)
                .Recurse(i => i.SelectMany(j => schema.GetSubstitutionGroupMembers(j)))
                .SelectMany(i => i);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="element"/> is substitutable by <paramref name="by"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsSubstitutableBy(this XmlSchemaSet schema, XmlSchemaElement element, XmlSchemaElement by)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (by == null)
                throw new ArgumentNullException(nameof(by));

            return GetSubstitutionElements(schema, element).Any(i => i.QualifiedName == by.QualifiedName);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="element"/> is substituted by <paramref name="by"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool IsSubstitutableBy(this XmlSchemaSet schema, XName element, XName by)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (by == null)
                throw new ArgumentNullException(nameof(by));

            return IsSubstitutableBy(schema, GetElement(schema, element), GetElement(schema, by));
        }

    }

}
