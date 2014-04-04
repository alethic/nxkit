using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for an interface which provides attributes of an element.
    /// </summary>
    public abstract class AttributesProvider
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AttributesProvider(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAttribute(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            var fq = element.Attribute(Constants.XForms_1_0 + name);
            if (fq != null)
                return (string)fq;

            var ln = element.Name.Namespace == Constants.XForms_1_0 ? element.Attribute(name) : null;
            if (ln != null)
                return (string)ln;

            return null;
        }

    }

}
