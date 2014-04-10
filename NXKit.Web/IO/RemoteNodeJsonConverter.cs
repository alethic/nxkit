using System;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Xml;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a JSON converter for rendering document nodes.
    /// </summary>
    public abstract class RemoteNodeJsonConverter :
        RemoteObjectJsonConverter
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var node = value as XNode;
            if (node == null)
                throw new JsonWriterException();

            WriteJson(writer, node, serializer);
        }

        void WriteJson(JsonWriter writer, XNode node, JsonSerializer serializer)
        {
            if (node.Host() == null)
                throw new JsonWriterException();

            var jobj = GetObject(node, serializer);
            if (jobj == null)
                throw new JsonWriterException();

            jobj.WriteTo(writer);
        }

        protected virtual JObject GetObject(XNode node, JsonSerializer serializer)
        {
            return ToObject(node.Interfaces());
        }

    }

}
