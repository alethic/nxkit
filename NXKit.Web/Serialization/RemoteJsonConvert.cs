using System.Xml.Linq;

using Newtonsoft.Json;

namespace NXKit.Web.Serialization
{

    /// <summary>
    /// Provides methods to write a remote object stream.
    /// </summary>
    public static class RemoteJsonConvert
    {

        static readonly JsonConverter[] converters = new JsonConverter[] {
            new XNameJsonConverter(),
            new RemoteXTextJsonConverter(),
            new RemoteObjectJsonConverter(),
            new RemoteXElementJsonConverter(),
        };

        static readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        static RemoteJsonConvert()
        {
            serializer = new JsonSerializer();

            foreach (var converter in converters)
                serializer.Converters.Add(converter);
        }

        public static void WriteTo(JsonWriter writer, XElement element)
        {
            serializer.Serialize(writer, element);
        }

    }

}
