using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.XInclude
{

    [Extension("{http://www.w3.org/2001/XInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeProperties :
        ElementExtension
    {

        readonly IncludeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public IncludeProperties(XElement element, IncludeAttributes attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);

            this.attributes = attributes;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public IncludeProperties(XElement element)
            : this(element, element.AnnotationOrCreate<IncludeAttributes>(() => new IncludeAttributes(element)))
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public virtual Uri Href
        {
            get { return !string.IsNullOrWhiteSpace(attributes.Href) ? new Uri(attributes.Href, UriKind.RelativeOrAbsolute) : null; }
        }

        public virtual IncludeParse Parse
        {
            get { return !string.IsNullOrWhiteSpace(attributes.Parse) ? (IncludeParse)Enum.Parse(typeof(IncludeParse), attributes.Parse) : IncludeParse.Xml; }
        }

        public virtual string XPointer
        {
            get { return attributes.XPointer; }
        }

        public virtual Encoding Encoding
        {
            get { return !string.IsNullOrWhiteSpace(attributes.Encoding) ? System.Text.Encoding.GetEncoding(attributes.Encoding) : null; }
        }

        public virtual MediaRangeList Accept
        {
            get { return attributes.Accept; }
        }

        public virtual string AcceptLanguage
        {
            get { return attributes.AcceptLanguage; }
        }

    }

}
