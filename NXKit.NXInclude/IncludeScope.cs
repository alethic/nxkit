using System;
using System.Xml.Linq;

namespace NXKit.NXInclude
{

    [Extension(PredicateType = typeof(IncludeScopePredicate))]
    public class IncludeScope :
        ElementExtension,
        IRefScope
    {

        public class IncludeScopePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
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
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
