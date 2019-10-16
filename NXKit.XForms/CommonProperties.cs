using System;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms common properties.
    /// </summary>
    [Extension(typeof(CommonProperties))]
    public class CommonProperties :
        ElementExtension
    {

        readonly CommonAttributes attributes;
        readonly IExport<EvaluationContextResolver> contextResolver;
        readonly Lazy<XPathExpression> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="contextResolver"></param>
        public CommonProperties(
            XElement element,
            CommonAttributes attributes,
            IExport<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));

            context = new Lazy<XPathExpression>(() => !string.IsNullOrEmpty(attributes.Context) ? contextResolver.Value.Context.CompileXPath(element, attributes.Context) : null);
        }

        public IdRef Model
        {
            get { return attributes.Model; }
        }

        public XPathExpression Context
        {
            get { return context.Value; }
        }

    }

}