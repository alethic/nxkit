using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web
{

    public class ViewMessageCommandJsonConverter :
        JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ViewMessageCommand);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);

            if (jobj.Value<string>("Action") == "Update")
                return jobj.Property("Args").Value.ToObject<ViewMessageUpdateCommand>(serializer);

            if (jobj.Value<string>("Action") == "Invoke")
                return jobj.Property("Args").Value.ToObject<ViewMessageInvokeCommand>(serializer);

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }

}
