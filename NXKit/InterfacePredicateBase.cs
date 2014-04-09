using System;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base implementation of <see cref="IInterfacePredicate"/>.
    /// </summary>
    public abstract class InterfacePredicateBase :
        IInterfacePredicate
    {

        /// <summary>
        /// Returns <c>true</c> if the interface type specified by <paramref name="type"/> should be applied to the
        /// <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract bool IsMatch(XObject obj, Type type);

    }

}
