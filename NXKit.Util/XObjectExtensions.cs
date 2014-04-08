using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Provides extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

        /// <summary>
        /// Obtains the ancestors of the given <see cref="XObject"/>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> Ancestors(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Parent != null ? self.Parent.AncestorsAndSelf() : Enumerable.Empty<XElement>();
        }

        /// <summary>
        /// Obtains the ancestors of the given <see cref="XObject"/> filtered by name.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> Ancestors(this XObject self, XName name)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(name != null);

            return self.Parent != null ? self.Parent.AncestorsAndSelf(name) : Enumerable.Empty<XElement>();
        }

        /// <summary>
        /// 
        /// Obtains the ancestors of the given <see cref="XObject"/>, including the specified instance.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<XObject> AncestorsAndSelf(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Ancestors().Prepend(self);
        }

    }

}
