using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Marks an annotation as supporting direct serialization.
    /// </summary>
    public interface ISerializableAnnotation
    {

        /// <summary>
        /// Serializes data to a <see cref="XElement"/>.
        /// </summary>
        /// <param name="serializer"></param>
        XElement Serialize(AnnotationSerializer serializer);

        /// <summary>
        /// Deserializes data from the given <see cref="XElement"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="element"></param>
        void Deserialize(AnnotationSerializer serializer, XElement element);

    }

}
