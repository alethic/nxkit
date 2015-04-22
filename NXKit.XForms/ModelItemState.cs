using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit.XForms
{

    /// <summary>
    /// Records additional information associated with a model item.
    /// </summary>
    [SerializableAnnotation]
    public class ModelItemState :
        IAttributeSerializableAnnotation
    {

        XName type;
        bool? readOnly;
        bool? required;
        bool? relevant;
        bool? constraint;
        bool? valid;

        public XName Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool? ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool? Required
        {
            get { return required; }
            set { required = value; }
        }

        public bool? Relevant
        {
            get { return relevant; }
            set { relevant = value; }
        }

        public bool? Constraint
        {
            get { return constraint; }
            set { constraint = value; }
        }

        public bool? Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        IEnumerable<XAttribute> IAttributeSerializableAnnotation.Serialize(AnnotationSerializer serializer, XNamespace ns)
        {
            if (type != null)
                yield return new XAttribute(ns + "type", type);
            if (readOnly != null)
                yield return new XAttribute(ns + "readonly", readOnly);
            if (required != null)
                yield return new XAttribute(ns + "required", required);
            if (relevant != null)
                yield return new XAttribute(ns + "relevant", relevant);
            if (constraint != null)
                yield return new XAttribute(ns + "constraint", constraint);
            if (valid != null)
                yield return new XAttribute(ns + "valid", valid);
        }

        void IAttributeSerializableAnnotation.Deserialize(AnnotationSerializer serializer, XNamespace ns, IEnumerable<XAttribute> attributes)
        {
            type = attributes.Where(i => i.Name == ns + "type").Select(i => XName.Get((string)i)).FirstOrDefault();
            readOnly = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "readonly");
            required = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "required");
            relevant = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "relevant");
            constraint = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "constraint");
            valid = (bool?)attributes.FirstOrDefault(i => i.Name == ns + "valid");
        }

    }

}
