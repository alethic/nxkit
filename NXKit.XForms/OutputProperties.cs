using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.IO.Media;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'output' properties.
    /// </summary>
    [Extension(typeof(OutputProperties), "{http://www.w3.org/2002/xforms}output")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class OutputProperties :
        ElementExtension
    {

        readonly OutputAttributes attributes;
        readonly Extension<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> value;
        readonly MediaRange mediaType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public OutputProperties(
            XElement element,
            OutputAttributes attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.value = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Value) ? context.Value.Context.CompileXPath(element, attributes.Value) : null);
        }

        public XPathExpression Value
        {
            get { return value.Value; }
        }

        public MediaRange MediaType
        {
            get { return attributes.MediaType; }
        }

        public string Appearance
        {
            get { return attributes.Appearance; }
        }

    }

}