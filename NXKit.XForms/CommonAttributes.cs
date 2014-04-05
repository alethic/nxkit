using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// 3.3.1 Common Attributes
    /// 
    /// The Common Attribute Collection applies to every element in the XForms namespace.
    /// </summary>
    public class CommonAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public CommonAttributes(NXElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'model' attribute value.
        /// </summary>
        public string Model
        {
            get { return GetAttribute("model"); }
        }

        /// <summary>
        /// Gets the 'context' attribute value.
        /// </summary>
        public string Context
        {
            get { return GetAttribute("context"); }
        }

    }

}
