using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms common properties.
    /// </summary>
    [Interface]
    public class ActionProperties :
        ElementExtension
    {

        readonly ActionAttributes attributes;
        readonly Lazy<EvaluationContextResolver> contextResolver;
        readonly Lazy<XPathExpression> while_;
        readonly Lazy<XPathExpression> if_;
        readonly Lazy<XPathExpression> iterate;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="contextResolver"></param>
        public ActionProperties(
            XElement element,
            Lazy<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.attributes = element.AnnotationOrCreate<ActionAttributes>(() => new ActionAttributes(element));
            this.contextResolver = contextResolver;

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