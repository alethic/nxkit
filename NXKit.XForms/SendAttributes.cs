using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'send' element.
    /// </summary>
    public class SendAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SendAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "send");
        }

        /// <summary>
        /// Gets the 'if' attribute values.
        /// </summary>
        public IdRef Submission
        {
            get { return GetAttributeValue("submission"); }
        }

    }

}