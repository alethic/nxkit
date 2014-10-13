using System;

using Newtonsoft.Json;

namespace NXKit.View.Server
{

    public class ViewMessageJsonConverter :
        JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(ViewMessage).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.TypeNameHandling = TypeNameHandling.All;
            return serializer.Deserialize<ViewMessage>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

    }

}
