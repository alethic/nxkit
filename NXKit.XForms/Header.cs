using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("http://www.w3.org/2002/xforms", "header")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Header :
        ElementExtension
    {

        readonly Extension<HeaderAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public Header(
            XElement element,
            Extension<HeaderAttributes> attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = attributes;
        }

        internal IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders()
        {
            yield break;
        }

    }

}
