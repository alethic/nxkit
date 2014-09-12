using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Describes an interface capable of deserializing annotations.
    /// </summary>
    public interface IAnnotationObjectDeserializer
    {



    }

    /// <summary>
    /// Describes an interface capable of deserializing annotations for the specified <see cref="XObject"/> type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnnotationDeserializer<T> :
        IAnnotationObjectDeserializer
        where T : XObject
    {

        /// <summary>
        /// Serializes data from <paramref name="src"/> to <paramref name="dst"/>.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        void Deserialize(T src, T dst);

    }

}
