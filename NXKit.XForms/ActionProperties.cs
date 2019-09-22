using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms common properties.
    /// </summary>
    [Extension(typeof(ActionProperties), "{http://www.w3.org/2001/xml-events}action")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ActionProperties :
        ElementExtension
    {

        readonly ActionAttributes attributes;
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
            ActionAttributes attributes,
            Extension<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));

            this.while_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.White) ? contextResolver.Value.Context.CompileXPath(element, attributes.White) : null);

            this.if_ = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.If) ? contextResolver.Value.Context.CompileXPath(element, attributes.If) : null);

            this.iterate = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Iterate) ? contextResolver.Value.Context.CompileXPath(element, attributes.Iterate) : null);
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