namespace NXKit.View.Windows.Extensions
{

    /// <summary>
    /// Describes metadata available to the View layer.
    /// </summary>
    public interface IViewMetadata
    {

        /// <summary>
        /// Gets whether or not the <see cref="XElement"/> is a metadata element.
        /// </summary>
        bool IsMetadata { get; }

    }

}
