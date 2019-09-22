using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("http://www.w3.org/2002/xforms", "header")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Header :
        ElementExtension
    {

        readonly HeaderAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public Header(
            XElement element,
            HeaderAttributes attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
        }

        internal IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders()
        {
            yield break;
        }

    }

}
