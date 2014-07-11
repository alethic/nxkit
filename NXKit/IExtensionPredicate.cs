using System;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Describes a type that provides filtering capabilities to the <see cref="InterfaceAttribute"/> resolver.
    /// </summary>
    public interface IExtensionPredicate
    {

        /// <summary>
        /// Returns <c>true</c> if the interface type specified by <paramref name="type"/> should be applied to the
        /// <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsMatch(XObject obj, Type type);

    }

}
