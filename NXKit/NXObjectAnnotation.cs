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
    public class NXObjectAnnotation :
        IAttributeSerializableAnnotation
    {

        bool init;
        bool load;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXObjectAnnotation()
        {
            this.init = true;
            this.load = true;
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
            if (init)
                yield return new XAttribute(ns + "init", init);
        }

        void IAttributeSerializableAnnotation.Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes)
        {
            init = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "init") ?? false;
        }

    }

}
