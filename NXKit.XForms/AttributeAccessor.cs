using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base class for an interface which provides XForms attributes of a specific element.
    /// </summary>
    public abstract class AttributeAccessor
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AttributeAccessor(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }


        public XAttribute GetAttribute(string name)
        {
            var fq = element.Attribute(Constants.XForms_1_0 + name);
            if (fq != null)
                return fq;

            var ln = element.Name.Namespace == Constants.XForms_1_0 ? element.Attribute(name) : null;
            if (ln != null)
                return ln;

            return null;
        }

        /// <summary>
        /// Gets the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAttributeValue(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            return (string)GetAttribute(name);
        }

        /// <summary>
        /// Sets the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute(string name, string value)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentNullException>(value != null);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the XForms attribute of the specified name.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveAttribute(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            throw new NotImplementedException();
        }

    }

}
