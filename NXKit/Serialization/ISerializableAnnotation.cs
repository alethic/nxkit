using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Marks an annotation as supporting direct serialization.
    /// </summary>
    [ContractClass(typeof(ISerializableAnnotation_Contract))]
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

    [ContractClassFor(typeof(ISerializableAnnotation))]
    abstract class ISerializableAnnotation_Contract :
        ISerializableAnnotation
    {

        public XElement Serialize(AnnotationSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);
            throw new NotImplementedException();
        }

        public void Deserialize(AnnotationSerializer serializer, XElement element)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);
            Contract.Requires<ArgumentNullException>(element != null);
            throw new NotImplementedException();
        }

    }

}
