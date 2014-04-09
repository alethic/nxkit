using System.Xml;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    public class ModelState
    {

        [XmlAttribute]
        public bool Construct { get; set; }

        [XmlAttribute]
        public bool ConstructDone { get; set; }

        [XmlAttribute]
        public bool Ready { get; set; }

        [XmlAttribute]
        public bool RebuildFlag { get; set; }

        [XmlAttribute]
        public bool RecalculateFlag { get; set; }

        [XmlAttribute]
        public bool RevalidateFlag { get; set; }

        [XmlAttribute]
        public bool RefreshFlag { get; set; }
    }

}
