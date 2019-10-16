using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'model' attributes.
    /// </summary>
    [Extension(typeof(ModelAttributes), "{http://www.w3.org/2002/xforms}model")]
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
            if (element == null)
                throw new ArgumentNullException(nameof(element));
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

        public string Functions
        {
            get { return GetAttributeValue("functions"); }
        }

    }

}