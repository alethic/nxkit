using System;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms common properties.
    /// </summary>
    [Extension(typeof(ActionProperties), "{http://www.w3.org/2001/xml-events}action")]
    public class ActionProperties :
        ElementExtension
    {

        readonly ActionAttributes attributes;
        readonly IExport<EvaluationContextResolver> contextResolver;
        readonly Lazy<XPathExpression> while_;
        readonly Lazy<XPathExpression> if_;
        readonly Lazy<XPathExpression> iterate;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="contextResolver"></param>
        public ActionProperties(
            XElement element,
            ActionAttributes attributes,
            IExport<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));

            while_ = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.White) ? contextResolver.Value.Context.CompileXPath(element, attributes.White) : null);
            if_ = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.If) ? contextResolver.Value.Context.CompileXPath(element, attributes.If) : null);
            iterate = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Iterate) ? contextResolver.Value.Context.CompileXPath(element, attributes.Iterate) : null);
        }

        public XPathExpression While => while_.Value;

        public XPathExpression If => if_.Value;

        public XPathExpression Iterate => iterate.Value;

    }

}