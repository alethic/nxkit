using System.Xml.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("document")]
    public class DocumentAnnotation
    {

        [XmlAttribute("construct-done-once")]
        public bool ConstructDoneOnce { get; set; }

        [XmlAttribute("failed")]
        public bool Failed { get; set; }

    }

}
