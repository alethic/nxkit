using System;
using System.Text;
using System.Xml.Linq;

using NXKit.IO.Media;

namespace NXKit.XInclude
{

    [Extension(typeof(IncludeProperties))]
    public class IncludeProperties :
        ElementExtension
    {

        readonly Func<IncludeAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public IncludeProperties(
            XElement element, 
            IncludeAttributes attributes)
            : this(element, () => attributes)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));
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
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));

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
