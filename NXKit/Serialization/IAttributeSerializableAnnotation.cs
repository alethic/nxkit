using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Serialization
{

    /// <summary>
    /// Marks an annotation as supporting serialization to attributes.
    /// </summary>
    [ContractClass(typeof(IAttributeSerializableAnnotation_Contract))]
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

    [ContractClassFor(typeof(IAttributeSerializableAnnotation))]
    abstract class IAttributeSerializableAnnotation_Contract :
        IAttributeSerializableAnnotation
    {
        
        public IEnumerable<XAttribute> Serialize(AnnotationSerializer serializer, XNamespace ns)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);
            Contract.Requires<ArgumentNullException>(ns != null);
            throw new NotImplementedException();
        }

        public void Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes)
        {
            Contract.Requires<ArgumentNullException>(serializer != null);
            Contract.Requires<ArgumentNullException>(ns != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            throw new NotImplementedException();
        }

    }

}
