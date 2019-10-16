using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'value' element attributes.
    /// </summary>
    [Extension(typeof(ItemValueAttributes), "{http://www.w3.org/2002/xforms}value", PredicateType = typeof(ItemValuePredicate))]
    public class ItemValueAttributes :
        AttributeAccessor
    {

        class ItemValuePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "item";
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ItemValueAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'value' attribute.
        /// </summary>
        public XAttribute ValueAttribute
        {
            get { return GetAttribute("value"); }
        }

        /// <summary>
        /// Gets the 'value' attribute values.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

    }

}