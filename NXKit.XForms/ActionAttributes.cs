using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms action attributes.
    /// </summary>
    public class ActionAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ActionAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'if' attribute values.
        /// </summary>
        public string If
        {
            get { return GetAttributeValue("if"); }
        }

        /// <summary>
        /// Gets the 'while' attribute values.
        /// </summary>
        public string White
        {
            get { return GetAttributeValue("while"); }
        }

        /// <summary>
        /// Gets the 'iterate' attribute values.
        /// </summary>
        public string Iterate
        {
            get { return GetAttributeValue("iterate"); }
        }

    }

}