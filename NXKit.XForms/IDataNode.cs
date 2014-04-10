using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes a UI element.
    /// </summary>
    [Remote]
    public interface IDataNode
    {

        /// <summary>
        /// Gets the data type of the node.
        /// </summary>
        [Remote]
        XName DataType { get; }

        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        [Remote]
        string Value { get; set; }

        /// <summary>
        /// Gets whether or not the node's value is valid.
        /// </summary>
        [Remote]
        bool Valid { get; }

    }

}
