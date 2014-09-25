using System;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NXKit.Server.Serialization
{

    /// <summary>
    /// Provides a JSON converter for rendering document comments.
    /// </summary>
    public class RemoteXCommentJsonConverter :
        RemoteXNodeJsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(XComment).IsAssignableFrom(objectType);
        }

        protected override void Apply(XNode node, JsonSerializer serializer, JObject obj)
        {
            var comment = node as XComment;
            if (comment == null)
                throw new JsonException();

            base.Apply(node, serializer, obj);
            obj["Type"] = "Comment";
            obj["Value"] = comment.Value;
        }

    }

}
