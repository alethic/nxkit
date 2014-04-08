using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides extension methods for <see cref="XElement"/> instances.
    /// </summary>
    public static class XElementExtensions
    {

        /// <summary>
        /// Gets all the implemented interfaces of the given <see cref="XElement"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XElement self, string id)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Host() != null);

            // discover the root visual
            var root = self.AncestorsAndSelf()
                .First(i => i != null && i.Parent == null);

            // naming scope of current element
            var namingScopes = self
                .Ancestors()
                .SelectMany(i => i.Interfaces<INamingScope>())
                .ToArray();


            throw new NotImplementedException();

            //// search all descendents of the root element that are sharing naming scopes with myself
            //foreach (var visual in root.DescendantsIncludeNS(namingScopes).OfType<NXElement>())
            //    if (visual.Id == id)
            //        return visual;

            //return null;
        }

    }

}
