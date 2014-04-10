using System;
using System.Linq;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides a JSON converter for rendering document nodes.
    /// </summary>
    public class RemoteElementJsonConverter :
        RemoteNodeJsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(XElement).IsAssignableFrom(objectType);
        }

        protected override JObject GetObject(XNode node, JsonSerializer serializer)
        {
            var element = node as XElement;
            if (element == null)
                return null;

            var obj = base.GetObject(element, serializer);
            if (obj == null)
                return null;

            // append children nodes
            obj.Add("Nodes", 
                new JArray(element.Nodes()
                    .Select(i => 
                        JObject.FromObject(i, serializer))));

            return obj;
        }

    }

}
