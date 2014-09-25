using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace NXKit.Server.Serialization
{

    /// <summary>
    /// Custom JSON serializer for handling the client push API.
    /// </summary>
    public class RemoteJsonSerializer :
        JsonSerializer
    {

        static internal readonly JsonConverter[] converters = new JsonConverter[] {
            new XNameJsonConverter(),
            new RemoteXTextJsonConverter(),
            new RemoteObjectJsonConverter(),
            new RemoteXElementJsonConverter(),
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RemoteJsonSerializer()
            : base()
        {
            MissingMemberHandling = MissingMemberHandling.Error;
            NullValueHandling = NullValueHandling.Include;
            ObjectCreationHandling = ObjectCreationHandling.Auto;
            TypeNameHandling = TypeNameHandling.Objects;

            foreach (var converter in converters)
                Converters.Add(converter);
        }

    }

}
