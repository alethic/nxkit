using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Describes an interface capable of serializing annotations.
    /// </summary>
    public interface IAnnotationObjectSerializer
    {



    }

    /// <summary>
    /// Describes an interface capable of serializing annotations for the specified <see cref="XObject"/> type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnnotationSerializer<T> :
        IAnnotationObjectSerializer
        where T : XObject
    {

        /// <summary>
        /// Serializes data from <paramref name="src"/> to <paramref name="dst"/>.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        void Serialize(T src, T dst);

    }

}
