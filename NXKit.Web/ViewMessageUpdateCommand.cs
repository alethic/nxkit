using Newtonsoft.Json.Linq;

namespace NXKit.Web
{

    public class ViewMessageUpdateCommand :
        ViewMessageCommand
    {

        /// <summary>
        /// Gets or sets the ID of the node to update.
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the interface name on the node to update.
        /// </summary>
        public string Interface { get; set; }

        /// <summary>
        /// Gets or sets the property name of the interface to update.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the value to update to.
        /// </summary>
        public JValue Value { get; set; }

    }

}
