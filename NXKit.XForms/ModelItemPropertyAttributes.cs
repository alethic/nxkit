using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the bind element.
    /// </summary>
    public class ModelItemPropertyAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ModelItemPropertyAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public string Type
        {
            get { return GetAttributeValue("type"); }
        }

        public string Calculate
        {
            get { return GetAttributeValue("calculate"); }
        }

        public string ReadOnly
        {
            get { return GetAttributeValue("readonly"); }
        }

        public string Required
        {
            get { return GetAttributeValue("required"); }
        }

        public string Relevant
        {
            get { return GetAttributeValue("relevant"); }
        }

        public string Constraint
        {
            get { return GetAttributeValue("constraint"); }
        }

        public string P3PType
        {
            get { return GetAttributeValue("p3ptype"); }
        }

    }

}
