using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the standard XForms binding attributes.
    /// </summary>
    [Extension(typeof(RepeatAttributes), "{http://www.w3.org/2002/xforms}repeat")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class RepeatAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public RepeatAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'ref' attribute values.
        /// </summary>
        public int StartIndex
        {
            get { return (int?)GetAttribute("startindex") ?? 1; }
        }

        /// <summary>
        /// Gets the 'nodeset' attribute values.
        /// </summary>
        public int? Number
        {
            get { return (int?)GetAttribute("number"); }
        }

    }

}