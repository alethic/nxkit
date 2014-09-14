using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms common properties.
    /// </summary>
    [Extension("{http://www.w3.org/2001/xml-events}action")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ActionProperties :
        ElementExtension
    {

        readonly Extension<ActionAttributes> attributes;
        readonly Extension<EvaluationContextResolver> contextResolver;
        readonly Lazy<XPathExpression> while_;
        readonly Lazy<XPathExpression> if_;
        readonly Lazy<XPathExpression> iterate;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="contextResolver"></param>
        [ImportingConstructor]
        public ActionProperties(
            XElement element,
            Extension<ActionAttributes> attributes,
            Extension<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.attributes = attributes;
            this.contextResolver = contextResolver;

            this.while_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.White) ? contextResolver.Value.Context.CompileXPath(element, attributes.Value.White) : null);

            this.if_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.If) ? contextResolver.Value.Context.CompileXPath(element, attributes.Value.If) : null);

            this.iterate = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.Iterate) ? contextResolver.Value.Context.CompileXPath(element, attributes.Value.Iterate) : null);
        }

        public XPathExpression While
        {
            get { return while_.Value; }
        }

        public XPathExpression If
        {
            get { return if_.Value; }
        }

        public XPathExpression Iterate
        {
            get { return iterate.Value; }
        }

    }

}