using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit.NXInclude
{

    [SerializableAnnotation]
    public class IncludeScopeAnnotation :
        ISerializableAnnotation
    {

        public XElement Serialize(AnnotationSerializer serializer)
        {
            return new XElement("include-scope");
        }

        public void Deserialize(AnnotationSerializer serializer, XElement element)
        {

        }

    }

}
