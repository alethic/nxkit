using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.Util
{

    public static class XObjectExtensions
    {

        public static IEnumerable<XElement> Ancestors(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Parent != null ? self.Parent.AncestorsAndSelf() : Enumerable.Empty<XElement>();
        }

        public static IEnumerable<XObject> AncestorsAndSelf(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Ancestors().Prepend(self);
        }

    }

}
