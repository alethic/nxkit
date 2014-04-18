using System;
using System.Linq;
using NXKit.Xml;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XInclude
{

    [Interface("{http://www.w3.org/2001/XInclude}include")]
    public class Include :
        ElementExtension,
        IOnInitialize
    {

        class IncludeAnnotation
        {

            readonly string source;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="source"></param>
            public IncludeAnnotation(string source)
            {
                this.source = source;
            }

            public string Source
            {
                get { return source; }
            }

        }

        readonly IncludeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Include(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new IncludeAttributes(element);
        }

        public void Initialize()
        {
            var uri = new Uri(attributes.Href ?? "", UriKind.RelativeOrAbsolute);
            if (Element.GetBaseUri() != null && !uri.IsAbsoluteUri)
                uri = new Uri(Element.GetBaseUri(), uri);

            var xml = XDocument.Load(uri.ToString(), LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri);
            if (xml != null)
            {
                // annotate element and replace self in graph
                var element = new XElement(xml.Root);
                element.AnnotationOrCreate<IncludeAnnotation>(() => new IncludeAnnotation(xml.BaseUri));
                Element.AddBeforeSelf(element);

                // initialize element and descendants
                var inits = element.DescendantNodesAndSelf()
                    .SelectMany(i => i.Interfaces<IOnInitialize>());
                foreach (var init in inits)
                    init.Initialize();
            }
        }

    }

}
