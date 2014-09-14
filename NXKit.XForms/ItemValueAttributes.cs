using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'value' element attributes.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}value", PredicateType = typeof(ItemValuePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ItemValueAttributes :
        AttributeAccessor
    {

        class ItemValuePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj, Type type)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "item";
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public ItemValueAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
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