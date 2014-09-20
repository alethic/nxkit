using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.IO.Media;

namespace NXKit.XInclude
{

    [Extension("{http://www.w3.org/2001/XInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IncludeProperties :
        ElementExtension
    {

        readonly Func<IncludeAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public IncludeProperties(
            XElement element, 
            Extension<IncludeAttributes> attributes)
            : this(element, () => attributes.Value)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public IncludeProperties(
            XElement element,
            Func<IncludeAttributes> attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);

            this.attributes = attributes;
        }

        public IncludeAttributes Attributes
        {
            get { return attributes(); }
        }

        public virtual Uri Href
        {
            get { return !string.IsNullOrWhiteSpace(Attributes.Href) ? new Uri(Attributes.Href, UriKind.RelativeOrAbsolute) : null; }
        }

        public virtual IncludeParse Parse
        {
            get { return !string.IsNullOrWhiteSpace(Attributes.Parse) ? (IncludeParse)Enum.Parse(typeof(IncludeParse), Attributes.Parse) : IncludeParse.Xml; }
        }

        public virtual string XPointer
        {
            get { return Attributes.XPointer; }
        }

        public virtual Encoding Encoding
        {
            get { return !string.IsNullOrWhiteSpace(Attributes.Encoding) ? System.Text.Encoding.GetEncoding(Attributes.Encoding) : null; }
        }

        public virtual MediaRangeList Accept
        {
            get { return Attributes.Accept; }
        }

        public virtual string AcceptLanguage
        {
            get { return Attributes.AcceptLanguage; }
        }

    }

}
