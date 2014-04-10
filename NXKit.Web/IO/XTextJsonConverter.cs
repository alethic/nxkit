using System;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a JSON converter for rendering document nodes.
    /// </summary>
    public class XTextJsonConverter :
        JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(XText).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = value as XText;
            if (text == null)
                throw new JsonException();

            writer.WriteStartObject();
            writer.WritePropertyName("Text");
            writer.WriteValue(text.Value);
            writer.WriteEndObject();
        }

    }

}
