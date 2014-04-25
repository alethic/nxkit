using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.XForms
{

    [SerializableAnnotation]
    [XmlRoot("document")]
    public class DocumentAnnotation
    {

        [XmlAttribute("construct-done-once")]
        public bool ConstructDoneOnce { get; set; }

        [XmlAttribute("failed")]
        public bool Failed { get; set; }

    }

}
