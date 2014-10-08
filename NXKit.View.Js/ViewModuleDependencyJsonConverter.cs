using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.View.Js
{

    public class ViewModuleDependencyJsonConverter :
        JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ViewModuleDependency);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ViewModuleDependency.Parse(JToken.ReadFrom(reader).Value<string>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

    }

}
