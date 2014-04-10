using System;
using System.Linq;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.Serialization
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

        protected override void Apply(XNode node, JsonSerializer serializer, JObject obj)
        {
            var element = node as XElement;
            if (element == null)
                return;

            obj["Type"] = "Element";
            obj["Name"] = JToken.FromObject(element.Name, serializer);
            base.Apply(element, serializer, obj);

            // append children nodes
            obj["Nodes"] = new JArray(
                element.Nodes()
                    .Select(i => 
                        JObject.FromObject(i, serializer)));
        }

    }

}
