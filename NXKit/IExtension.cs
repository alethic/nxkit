using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes an extension compatible with a <see cref="XObject"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtension
    {

        /// <summary>
        /// Gets the targetted <see cref="XObject"/>.
        /// </summary>
        XObject Object { get; }

    }

}
