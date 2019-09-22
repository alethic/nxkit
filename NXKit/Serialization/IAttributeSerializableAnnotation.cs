using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Marks an annotation as supporting serialization to attributes.
    /// </summary>
    public interface IAttributeSerializableAnnotation
    {

        /// <summary>
        /// Serializes data to a <see cref="XElement"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="ns"></param>
        IEnumerable<XAttribute> Serialize(AnnotationSerializer serializer, XNamespace ns);

        /// <summary>
        /// Deserializes data from the given <see cref="XElement"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="ns"></param>
        /// <param name="attributes"></param>
        void Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes);

    }

}
