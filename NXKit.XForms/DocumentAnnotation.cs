using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit.XForms
{

    [SerializableAnnotation]
    public class DocumentAnnotation :
        ISerializableAnnotation
    {

        bool constructDoneOnce;
        bool failed;

        public bool ConstructDoneOnce
        {
            get => constructDoneOnce;
            set => constructDoneOnce = value;
        }

        public bool Failed
        {
            get => failed;
            set => failed = value;
        }

        void ISerializableAnnotation.Deserialize(AnnotationSerializer serializer, XElement element)
        {
            constructDoneOnce = (bool)element.Attribute("construct-done-once");
            failed = (bool)element.Attribute("failed");
        }

        XElement ISerializableAnnotation.Serialize(AnnotationSerializer serializer)
        {
            return new XElement("document",
                new XAttribute("construct-done-once", constructDoneOnce),
                new XAttribute("failed", failed ? "true" : "false"));
        }

    }

}
