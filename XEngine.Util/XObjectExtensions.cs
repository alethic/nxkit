using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XEngine.Util
{

    public static class XObjectExtensions
    {

        public static IEnumerable<XElement> Ancestors(this XObject self)
        {
            return self.Parent != null ? self.Parent.AncestorsAndSelf() : Enumerable.Empty<XElement>();
        }

        public static IEnumerable<XObject> AncestorsAndSelf(this XObject self)
        {
            return self.Ancestors().Prepend(self);
        }

    }

}
