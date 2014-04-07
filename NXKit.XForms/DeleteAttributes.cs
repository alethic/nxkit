using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'delete' attributes.
    /// </summary>
    public class DeleteAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public DeleteAttributes(NXElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'ref' attribute values.
        /// </summary>
        public string At
        {
            get { return GetAttributeValue("at"); }
        }

    }

}