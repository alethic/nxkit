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

        [XmlAttribute("construct")]
        public bool Construct { get; set; }

        [XmlAttribute("construct-done")]
        public bool ConstructDone { get; set; }

        [XmlAttribute("ready")]
        public bool Ready { get; set; }

        [XmlAttribute("rebuild")]
        public bool Rebuild { get; set; }

        [XmlAttribute("recalculate")]
        public bool Recalculate { get; set; }

        [XmlAttribute("revalidate")]
        public bool Revalidate { get; set; }

        [XmlAttribute("refresh")]
        public bool Refresh { get; set; }

        [XmlElement("schemas")]
        public XmlSchemaSet XmlSchemas { get; set; } = new XmlSchemaSet();

        XElement ISerializableAnnotation.Serialize(AnnotationSerializer serializer)
        {
            return new XElement("model",
                new XAttribute("construct", Construct),
                new XAttribute("construct-done", ConstructDone),
                new XAttribute("ready", Ready),
                new XAttribute("rebuild", Rebuild),
                new XAttribute("recalculate", Recalculate),
                new XAttribute("revalidate", Revalidate),
                new XAttribute("refresh", Refresh),
                new XElement("schemas", SerializeXmlSchemas()));
        }

        IEnumerable<XElement> SerializeXmlSchemas()
        {
            foreach (XmlSchema schema in XmlSchemas.Schemas())
            {
                var e = new XDocument();
                using (var wrt = e.CreateWriter())
                    schema.Write(wrt);
                yield return e.Root;
            }
        }

        void ISerializableAnnotation.Deserialize(AnnotationSerializer serializer, XElement element)
        {
            Construct = (bool)element.Attribute("construct");
            ConstructDone = (bool)element.Attribute("construct-done");
            Ready = (bool)element.Attribute("ready");
            Rebuild = (bool)element.Attribute("rebuild");
            Recalculate = (bool)element.Attribute("recalculate");
            Revalidate = (bool)element.Attribute("revalidate");
            Refresh = (bool)element.Attribute("refresh");
            DeserializeXmlSchemas(element.Element("schemas"));
        }

        void DeserializeXmlSchemas(XElement elements)
        {
            foreach (var element in elements.Elements())
                using (var rdr = element.CreateReader())
                    XmlSchemas.Add(XmlSchema.Read(rdr, (s, a) => { }));
        }

    }

}
