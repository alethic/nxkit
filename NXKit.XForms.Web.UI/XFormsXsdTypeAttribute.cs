using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Marks a type as handling or being associated with a given XSD type description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XFormsXsdTypeAttribute :
        Attribute
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> contains an attribute declaring it supports the
        /// specified XSD type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xsdType"></param>
        /// <returns></returns>
        public static bool Predicate(Type type, XName xsdType)
        {
            // control specifies no specific types
            var a = type.GetCustomAttributes<XFormsXsdTypeAttribute>(false).ToList();
            if (a.Count == 0)
                return true;

            // filter for matching types
            return a.Any(i => i.Name == xsdType);
        }

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> contains an attribute declaring it supporst the
        /// specified binding's XSD type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static bool Predicate(Type type, XFormsBinding binding)
        {
            return Predicate(type, binding != null && binding.Type != null ? binding.Type : null);
        }

        readonly XName name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public XFormsXsdTypeAttribute(string namespaceName, string localName)
        {
            Contract.Requires<ArgumentNullException>(namespaceName != null);
            Contract.Requires<ArgumentNullException>(localName != null);

            this.name = XName.Get(localName, namespaceName);
        }

        /// <summary>
        /// Gets the XSD type name.
        /// </summary>
        public XName Name
        {
            get { return name; }
        }

    }

}
