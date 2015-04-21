using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.XForms
{

    [SerializableAnnotation]
    public class ModelState :
        ISerializableAnnotation
    {

        bool construct;
        bool constructDone;
        bool ready;
        bool rebuild;
        bool recalculate;
        bool revalidate;
        bool refresh;
        XmlSchemaSet schemas = new XmlSchemaSet();

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

        [XmlElement("schemas")]
        public XmlSchemaSet XmlSchemas
        {
            get { return schemas; }
            set { schemas = value; }
        }

        XElement ISerializableAnnotation.Serialize(AnnotationSerializer serializer)
        {
            return new XElement("model",
                new XAttribute("construct", construct),
                new XAttribute("construct-done", constructDone),
                new XAttribute("ready", ready),
                new XAttribute("rebuild", rebuild),
                new XAttribute("recalculate", recalculate),
                new XAttribute("revalidate", revalidate),
                new XAttribute("refresh", refresh),
                new XElement("schemas", SerializeXmlSchemas()));
        }

        IEnumerable<XElement> SerializeXmlSchemas()
        {
            foreach (XmlSchema schema in schemas.Schemas())
            {
                var e = new XDocument();
                using (var wrt = e.CreateWriter())
                    schema.Write(wrt);
                yield return e.Root;
            }
        }

        void ISerializableAnnotation.Deserialize(AnnotationSerializer serializer, XElement element)
        {
            construct = (bool)element.Attribute("construct");
            constructDone = (bool)element.Attribute("construct-done");
            ready = (bool)element.Attribute("ready");
            rebuild = (bool)element.Attribute("rebuild");
            recalculate = (bool)element.Attribute("recalculate");
            revalidate = (bool)element.Attribute("revalidate");
            refresh = (bool)element.Attribute("refresh");
            DeserializeXmlSchemas(element.Element("schemas"));
        }

        void DeserializeXmlSchemas(XElement elements)
        {
            foreach (var element in elements.Elements())
                using (var rdr = element.CreateReader())
                    schemas.Add(XmlSchema.Read(rdr, (s, a) => { }));
        }

    }

}
