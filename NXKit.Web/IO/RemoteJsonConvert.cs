using System.Xml.Linq;
using Newtonsoft.Json;

namespace NXKit.Web.IO
{

    /// <summary>
    /// Provides methods to write a remote object stream.
    /// </summary>
    public static class RemoteJsonConvert
    {

        static readonly JsonConverter[] converters = new JsonConverter[] {
            new XNameJsonConverter(),
            new XTextJsonConverter(),
            new RemoteObjectJsonConverter(),
            new RemoteElementJsonConverter(),
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
