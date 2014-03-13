using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Marks a type as handling or being associated with a given appearance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XFormsAppearanceAttribute :
        Attribute
    {

        /// <summary>
        /// Returns <c>true</c> if the given <see cref="Type"/> contains an attribute declaring it supports the
        /// specified appearance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xsdType"></param>
        /// <returns></returns>
        public static bool Predicate(Type type, XName appearance)
        {
            // control specifies no specific types
            var a = type.GetCustomAttributes<XFormsAppearanceAttribute>(false).ToList();
            if (a.Count == 0)
                return true;

            // filter for matching types
            return a.Any(i => i.Name == appearance);
        }

        readonly XName name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="localName"></param>
        public XFormsAppearanceAttribute(string namespaceName, string localName)
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
