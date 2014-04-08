using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'instance' attributes.
    /// </summary>
    public class InstanceAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public InstanceAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'if' attribute values.
        /// </summary>
        public string Src
        {
            get { return GetAttributeValue("src"); }
        }

    }

}