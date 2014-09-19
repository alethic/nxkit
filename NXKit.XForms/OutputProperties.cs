using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.IO.Media;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'output' properties.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}output")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class OutputProperties :
        ElementExtension
    {

        readonly Extension<OutputAttributes> attributes;
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
            Extension<OutputAttributes> attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = attributes;
            this.context = context;

            this.value = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.Value) ? context.Value.Context.CompileXPath(element, attributes.Value.Value) : null);
        }

        public XPathExpression Value
        {
            get { return value.Value; }
        }

        public MediaRange MediaType
        {
            get { return attributes.Value.MediaType; }
        }

        public string Appearance
        {
            get { return attributes.Value.Appearance; }
        }

    }

}