using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various information on a <see cref="XDocument"/>.
    /// </summary>
    [SerializableAnnotation]
    public class DocumentAnnotation :
        IAttributeSerializableAnnotation
    {

        int nextObjectId = 1;

        /// <summary>
        /// Gets the next available object ID.
        /// </summary>
        public int GetNextObjectId()
        {
            return nextObjectId++;
        }
        
        IEnumerable<XAttribute> IAttributeSerializableAnnotation.Serialize(AnnotationSerializer serializer, XNamespace ns)
        {
            yield return new XAttribute(ns + "next-object-id", nextObjectId);
        }

        void IAttributeSerializableAnnotation.Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes)
        {
            nextObjectId = (int?)attributes.FirstOrDefault(i => i.Name == ns + "next-object-id") ?? 1;
        }

    }

}
