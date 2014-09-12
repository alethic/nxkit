using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

using NXKit.Composition;
using NXKit.Xml;

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

        readonly CommonAttributes attributes;
        readonly Lazy<EvaluationContextResolver> contextResolver;
        readonly IdRef model;
        readonly Lazy<XPathExpression> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="contextResolver"></param>
        public CommonProperties(
            XElement element,
            Lazy<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.attributes = element.AnnotationOrCreate<CommonAttributes>(() => new CommonAttributes(element));
            this.contextResolver = contextResolver;

            this.model = attributes.Model;

            this.context = new Lazy<XPathExpression>(() =>
                !string.IsNullOrEmpty(attributes.Context) ? contextResolver.Value.Context.CompileXPath(element, attributes.Context) : null);
        }

        public IdRef Model
        {
            get { return model; }
        }

        public XPathExpression Context
        {
            get { return context.Value; }
        }

    }

}