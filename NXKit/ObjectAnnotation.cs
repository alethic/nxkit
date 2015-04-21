using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various NXKit information on the <see cref="XObject"/>.
    /// </summary>
    [SerializableAnnotation]
    public class ObjectAnnotation :
        IAttributeSerializableAnnotation
    {

        int id;
        bool init;
        bool load;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ObjectAnnotation()
            : this(0)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        internal ObjectAnnotation(int id)
        {
            this.id = id;
            this.init = true;
            this.load = true;
        }

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        internal int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets if the init phase should be run.
        /// </summary>
        internal bool Init
        {
            get { return init; }
            set { init = value; }
        }

        /// <summary>
        /// Gets if the load phase should be run.
        /// </summary>
        internal bool Load
        {
            get { return load; }
            set { load = value; }
        }

        IEnumerable<XAttribute> IAttributeSerializableAnnotation.Serialize(AnnotationSerializer serializer, XNamespace ns)
        {
            yield return new XAttribute(ns + "id", id);
            yield return new XAttribute(ns + "init", init);
        }

        void IAttributeSerializableAnnotation.Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes)
        {
            id = (int?)attributes.FirstOrDefault(i => i.Name == ns + "id") ?? 0;
            init = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "init") ?? true;
        }

    }

}
