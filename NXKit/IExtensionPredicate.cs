using System;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes a type that provides filtering capabilities to the <see cref="ExtensionAttribute"/> resolver.
    /// </summary>
    public interface IExtensionPredicate
    {

        /// <summary>
        /// Returns <c>true</c> if the extension should be applied to the <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IsMatch(XObject obj);

    }

}
