using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="NXElement"/> instances.
    /// </summary>
    public static class NXElementsExtensions
    {

        /// <summary>
        /// Gets the attributes of the element.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<NXAttribute> Attributes(this NXElement self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.attributes;
        }

        /// <summary>
        /// Gets the attribute of the element with the specified name.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static NXAttribute Attribute(this NXElement self, XName name)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(name != null);

            return self.Attributes().FirstOrDefault(i => i.Name == name);
        }

    }

}
