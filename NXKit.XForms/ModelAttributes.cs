using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'model' attributes.
    /// </summary>
    public class ModelAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ModelAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'version' attribute values.
        /// </summary>
        public string Version
        {
            get { return GetAttributeValue("version"); }
        }

        public string Schema
        {
            get { return GetAttributeValue("schema"); }
        }

    }

}