using System;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace NXKit.Web.IO
{

    public class XNameJsonConverter :
        JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(XName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return XName.Get(reader.ReadAsString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((XName)value).ToString());
        }

    }

}
