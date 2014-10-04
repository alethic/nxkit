using System;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.View.Server.Serialization
{

    /// <summary>
    /// Provides a JSON converter for rendering document text nodes.
    /// </summary>
    public class RemoteXTextJsonConverter :
        RemoteXNodeJsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(XText).IsAssignableFrom(objectType);
        }

        protected override void Apply(XNode node, JsonSerializer serializer, JObject obj)
        {
            var text = node as XText;
            if (text == null)
                throw new JsonException();

            base.Apply(node, serializer, obj);
            obj["Type"] = "Text";
            obj["Value"] = text.Value;
        }

    }

}
