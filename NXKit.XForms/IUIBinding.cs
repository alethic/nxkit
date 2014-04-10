using System.Xml.Linq;

namespace NXKit.XForms
{

    [Remote]
    public interface IUIBinding
    {

        /// <summary>
        /// Gets the current data type of the interface.
        /// </summary>
        [Remote]
        XName DataType { get; }

        /// <summary>
        /// Gets whether or not the interface is considered relevant.
        /// </summary>
        [Remote]
        bool Relevant { get; }

        /// <summary>
        /// Gets whether or not the interface is considered read-only.
        /// </summary>
        [Remote]
        bool ReadOnly { get; }

        /// <summary>
        /// Gets whether or not the interface is considered required.
        /// </summary>
        [Remote]
        bool Required { get; }

        /// <summary>
        /// Gets whether or not the interface is considered valid.
        /// </summary>
        [Remote]
        bool Valid { get; }

        /// <summary>
        /// Gets the current value of the interface.
        /// </summary>
        [Remote]
        string Value { get; set; }

        /// <summary>
        /// Initiates a refresh of the binding.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Discards any pending events of the binding.
        /// </summary>
        void DiscardEvents();

        /// <summary>
        /// Dispatches any pending events of the binding.
        /// </summary>
        void DispatchEvents();

    }

}
