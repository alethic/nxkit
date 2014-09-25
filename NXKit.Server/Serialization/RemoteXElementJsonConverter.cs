using System;
using System.Linq;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Server.Serialization
{

    /// <summary>
    /// Provides a JSON converter for rendering document elements.
    /// </summary>
    public class RemoteXElementJsonConverter :
        RemoteXNodeJsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(XElement).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Serializes the given <see cref="XNode"/>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        JObject FromXNode(XNode node, JsonSerializer serializer)
        {
            // ignore empty text
            var text = node as XText;
            if (text != null)
                if (string.IsNullOrWhiteSpace(text.Value))
                    return null;

            return JObject.FromObject(node, serializer);
        }

        protected override void Apply(XNode node, JsonSerializer serializer, JObject obj)
        {
            var element = node as XElement;
            if (element == null)
                return;

            base.Apply(element, serializer, obj);
            obj["Name"] = JToken.FromObject(element.Name, serializer);
            obj["Type"] = "Element";

            // append children nodes
            obj["Nodes"] = new JArray(
                element.Nodes()
                    .Select(i => FromXNode(i, serializer))
                    .Where(i => i != null));
        }

    }

}
