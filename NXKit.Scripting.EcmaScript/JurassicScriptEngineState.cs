using System;
using System.Xml.Linq;

using NXKit.Serialization;

namespace NXKit.Scripting.EcmaScript
{

    [SerializableAnnotation]
    public class JurassicScriptEngineState :
        ISerializableAnnotation
    {

        internal byte[] state;

        public XElement Serialize(AnnotationSerializer serializer)
        {
            return new XElement("jurassic-engine", state != null ? Convert.ToBase64String(state) : null);
        }

        public void Deserialize(AnnotationSerializer serializer, XElement element)
        {
            state = element.Value != null ? Convert.FromBase64String(element.Value) : null;
        }

    }

}
