using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the bind element.
    /// </summary>
    public class BindAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public BindAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public XAttribute TypeAttribute
        {
            get { return GetAttribute("type"); }
        }

        public string Type
        {
            get { return GetAttributeValue("type"); }
        }

        public XAttribute CalculateAttribute
        {
            get { return GetAttribute("calculate"); }
        }

        public string Calculate
        {
            get { return GetAttributeValue("calculate"); }
        }

        public XAttribute ReadOnlyAttribute
        {
            get { return GetAttribute("readonly"); }
        }

        public string ReadOnly
        {
            get { return GetAttributeValue("readonly"); }
        }

        public XAttribute RequiredAttribute
        {
            get { return GetAttribute("required"); }
        }

        public string Required
        {
            get { return GetAttributeValue("required"); }
        }

        public XAttribute RelevantAttribute
        {
            get { return GetAttribute("relevant"); }
        }

        public string Relevant
        {
            get { return GetAttributeValue("relevant"); }
        }

        public XAttribute ConstraintAttribute
        {
            get { return GetAttribute("constraint"); }
        }

        public string Constraint
        {
            get { return GetAttributeValue("constraint"); }
        }

        public XAttribute P3PTypeAttribute
        {
            get { return GetAttribute("p3ptype"); }
        }

        public string P3PType
        {
            get { return GetAttributeValue("p3ptype"); }
        }

    }

}
