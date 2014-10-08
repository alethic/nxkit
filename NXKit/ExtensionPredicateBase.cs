using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base implementation of <see cref="IExtensionPredicate"/>.
    /// </summary>
    public abstract class ExtensionPredicateBase :
        IExtensionPredicate
    {

        /// <summary>
        /// Returns <c>true</c> if the extension should be applied to the <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool IsMatch(XObject obj);

    }

}
