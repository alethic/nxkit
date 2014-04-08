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
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XElement"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XElement self, string id)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Host() != null);

            // search referencable elements for matching id
            foreach (var element in RefElements(self))
                if ((string)element.Attribute("id") == id)
                    return element;

            return null;
        }

        /// <summary>
        /// Returns a filtered collection of <see cref="XElement"/>s within the containing <see cref="XDocument"/>
        /// which are reachable from the given <see cref="XElement"/> when considering ref scope.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> RefElements(this XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // obtain all scopes this element is a member of
            var scopes = new HashSet<IRefScope>(element
                .Ancestors()
                .SelectMany(i => i.Interfaces<IRefScope>()));

            // return elements that share one of these scopes
            return DescendantsAndSelfInRefScope(element.Document.Root, scopes);
        }

        /// <summary>
        /// Yields each descendant <see cref="XElement"/> of the specified <see cref="XElement"/> which falls within
        /// one of the specified <see cref="IRefScope"/>s.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        static IEnumerable<XElement> DescendantsAndSelfInRefScope(XElement self, HashSet<IRefScope> scopes)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(scopes != null);

            yield return self;

            // if this node establishes an unknown ref scope, ignore its descendants
            var scope = self.InterfaceOrDefault<IRefScope>();
            if (scope != null && !scopes.Contains(scope))
                yield break;

            // else, recurse into its descendants
            foreach (var element in self.Elements())
                foreach (var i in DescendantsAndSelfInRefScope(element, scopes))
                    yield return i;
        }

    }

}
