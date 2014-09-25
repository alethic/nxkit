using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NXKit.Xml;

namespace NXKit.Server.Serialization
{

    /// <summary>
    /// Provides a JSON converter for rendering document nodes.
    /// </summary>
    public abstract class RemoteXNodeJsonConverter :
        RemoteObjectJsonConverter
    {

        protected sealed override void Apply(object value, JsonSerializer serializer, JObject obj)
        {
            var node = value as XNode;
            if (node == null)
                throw new JsonException();

            obj["Id"] = node.GetObjectId();
            obj["Type"] = "Node";
            Apply(node, serializer, obj);
        }

        protected virtual void Apply(XNode node, JsonSerializer serializer, JObject obj)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(serializer != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            RemotesToObject(node.Interfaces<object>(), obj, serializer);
        }

    }

}
