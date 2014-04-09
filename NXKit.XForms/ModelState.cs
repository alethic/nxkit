using System.Xml;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("model")]
    public class ModelState
    {

        bool construct;
        bool constructDone;
        bool ready;
        bool rebuild;
        bool recalculate;
        bool revalidate;
        bool refresh;

        [XmlAttribute("construct")]
        public bool Construct
        {
            get { return construct; }
            set { construct = value; }
        }

        [XmlAttribute("construct-done")]
        public bool ConstructDone
        {
            get { return constructDone; }
            set { constructDone = value; }
        }

        [XmlAttribute("ready")]
        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        [XmlAttribute("rebuild")]
        public bool Rebuild
        {
            get { return rebuild; }
            set { rebuild = value; }
        }

        [XmlAttribute("recalculate")]
        public bool Recalculate
        {
            get { return recalculate; }
            set { recalculate = value; }
        }

        [XmlAttribute("revalidate")]
        public bool Revalidate
        {
            get { return revalidate; }
            set { revalidate = value; }
        }

        [XmlAttribute("refresh")]
        public bool Refresh
        {
            get { return refresh; }
            set { refresh = value; }
        }

    }

}
