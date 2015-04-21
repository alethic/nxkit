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
            return state != null && state.Length > 0 ? new XElement("jurassic-engine", Convert.ToBase64String(state)) : null;
        }

        public void Deserialize(AnnotationSerializer serializer, XElement element)
        {
            state = element.Value != null ? Convert.FromBase64String(element.Value) : null;
        }

    }

}
