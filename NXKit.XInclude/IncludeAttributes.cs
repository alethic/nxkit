using System;
using System.Xml.Linq;

namespace NXKit.XInclude
{

    [Extension(typeof(IncludeAttributes))]
    public class IncludeAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public IncludeAttributes(XElement element)
            : base(element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="defaultNamespace"></param>
        public IncludeAttributes(XElement element, XNamespace defaultNamespace)
            : base(element, defaultNamespace)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

        public string Href
        {
            get { return GetAttributeValue("href"); }
        }

        public string Parse
        {
            get { return GetAttributeValue("parse"); }
        }

        public string XPointer
        {
            get { return GetAttributeValue("xpointer"); }
        }

        public string Encoding
        {
            get { return GetAttributeValue("encoding"); }
        }

        public string Accept
        {
            get { return GetAttributeValue("accept"); }
        }

        public string AcceptLanguage
        {
            get { return GetAttributeValue("accept-language"); }
        }

    }

}
