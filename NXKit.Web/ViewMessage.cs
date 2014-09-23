using NXKit.Web.Commands;

using Newtonsoft.Json;

namespace NXKit.Web
{

    public class ViewMessage
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ViewMessage()
        {
            Status = ViewMessageStatus.Unknown;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="status"></param>
        public ViewMessage(ViewMessageStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="hash"></param>
        /// <param name="save"></param>
        /// <param name="node"></param>
        public ViewMessage(ViewMessageStatus status, string hash, string save, object node)
        {
            Status = status;
            Hash = hash;
            Save = save;
            Node = node;
        }

        /// <summary>
        /// Gets or sets the message status.
        /// </summary>
        public ViewMessageStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the SHA256 hash of the <see cref="Document"/> in the <see cref="IDocumentStore"/>.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the serialized <see cref="Document"/> state.
        /// </summary>
        public string Save { get; set; }

        /// <summary>
        /// Gets or sets the root remote node.
        /// </summary>
        public object Node { get; set; }

        /// <summary>
        /// Set of <see cref="ViewCommand"/>'s to be executed.
        /// </summary>
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
        public Command[] Commands { get; set; }

    }

}
