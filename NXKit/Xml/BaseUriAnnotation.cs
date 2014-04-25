using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.Xml
{

    /// <summary>
    /// Custom implementation of BaseUri since the built implementation is internal.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("base-uri")]
    public class BaseUriAnnotation :
        IXmlSerializable
    {

        Uri baseUri;

        /// <summary>
        /// Gets or sets the base URI value.
        /// </summary>
        internal Uri BaseUri
        {
            get { return baseUri; }
            set { baseUri = value; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "base-uri")
            {
                var baseUri_ = reader["uri"];
                if (baseUri_ != null)
                    baseUri = new Uri(baseUri_);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (baseUri != null)
                writer.WriteAttributeString("uri", baseUri.ToString());
        }

    }

}
