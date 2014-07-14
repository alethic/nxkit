using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.Util;
using NXKit.Xml;

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

        readonly OutputAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<XPathExpression> value;
        readonly MediaRange mediaType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public OutputProperties(
            XElement element,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = element.AnnotationOrCreate<OutputAttributes>(() => new OutputAttributes(element));
            this.context = context;

            this.value = new Lazy<XPathExpression>(() => 
                !string.IsNullOrEmpty(attributes.Value) ? context.Value.Context.CompileXPath(element, attributes.Value) : null);
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