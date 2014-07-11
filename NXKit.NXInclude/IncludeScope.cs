using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.NXInclude
{

    [Interface(typeof(IncludeScopePredicate))]
    public class IncludeScope :
        ElementExtension,
        IRefScope
    {

        public class IncludeScopePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj, Type type)
            {
                return obj.Annotation<IncludeScopeAnnotation>() != null;
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IncludeScope(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
