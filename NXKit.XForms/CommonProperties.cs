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
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class CommonProperties :
        ElementExtension
    {

        readonly Extension<CommonAttributes> attributes;
        readonly Extension<EvaluationContextResolver> contextResolver;
        readonly Lazy<XPathExpression> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="contextResolver"></param>
        [ImportingConstructor]
        public CommonProperties(
            XElement element,
            Extension<CommonAttributes> attributes,
            Extension<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.attributes = attributes;
            this.contextResolver = contextResolver;

            this.context = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Value.Context) ? contextResolver.Value.Context.CompileXPath(element, attributes.Value.Context) : null);
        }

        public IdRef Model
        {
            get { return attributes.Value.Model; }
        }

        public XPathExpression Context
        {
            get { return context.Value; }
        }

    }

}